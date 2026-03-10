# Deploy .NET API + Vue.js to AWS

Full-stack Instagram clone deployed to AWS using Terraform. Vue 3 frontend served via S3 + CloudFront, .NET 8 API running on ECS Fargate, SQL Server Express on RDS.

## Architecture

```
                         ┌──────────┐
                         │  Users   │
                         └────┬─────┘
                              │ HTTPS
                         ┌────▼─────┐
                         │CloudFront│
                         └──┬──┬──┬─┘
                 ┌──────────┘  │  └──────────┐
                 ▼             ▼              ▼
          S3 Frontend     ALB (/api/*)   S3 Uploads
          (Vue.js SPA)         │         (/uploads/*)
          default /*           │
                               │
                ┌──────────────┼──────────────┐
                │  VPC 10.0.0.0/16            │
                │              │              │
                │  ┌───────────▼──────────┐   │
                │  │   ECS Fargate        │   │
                │  │   .NET 8 API         │   │
                │  │   0.5 vCPU / 1GB     │   │
                │  └───────────┬──────────┘   │
                │              │              │
                │  ┌───────────▼──────────┐   │
                │  │   RDS SQL Server     │   │
                │  │   Express (Private)  │   │
                │  └──────────────────────┘   │
                └─────────────────────────────┘
```

### AWS Services

| Service | Purpose | Spec |
|---------|---------|------|
| CloudFront | CDN, HTTPS, routing | 3 behaviors: `/*`, `/api/*`, `/uploads/*` |
| S3 | Frontend hosting + image uploads | 2 buckets, OAC |
| ALB | Load balancer, health checks | Public subnets |
| ECS Fargate | .NET 8 API container | 0.5 vCPU, 1GB |
| RDS | SQL Server Express | db.t3.micro, private subnets |
| ECR | Docker image registry | Lifecycle: keep last 5 |
| SSM Parameter Store | Secrets (DB password, JWT key) | SecureString |
| CloudWatch | Container logs | 14-day retention |

### Terraform Modules

```
terraform/
├── main.tf              # Root module wiring
├── providers.tf         # AWS provider + S3 backend
├── variables.tf         # Input variables
├── outputs.tf           # Stack outputs
├── state.tf             # S3 + DynamoDB for remote state
├── acm.tf               # ACM certificate (optional, custom domain)
├── hardening.tf         # ALB security rules, CloudWatch alarms
└── modules/
    ├── networking/       # VPC, subnets (2 public + 2 private), IGW
    ├── ecr/              # Container registry + lifecycle policy
    ├── database/         # RDS SQL Server Express
    ├── secrets/          # SSM parameters
    ├── alb/              # Application Load Balancer
    ├── ecs/              # Fargate cluster, task definition, service
    ├── storage/          # S3 buckets (frontend + uploads)
    ├── cdn/              # CloudFront distribution + OAC
    └── dns/              # Route53 records (optional)
```

## Prerequisites

- [Terraform](https://www.terraform.io/) >= 1.5
- [AWS CLI](https://aws.amazon.com/cli/) configured with a named profile
- [Docker](https://www.docker.com/) for building the API image
- [Node.js](https://nodejs.org/) >= 18 for building the frontend

## Deployment Steps

### 1. Configure variables

```bash
cd terraform
cp terraform.tfvars.example terraform.tfvars
```

Edit `terraform.tfvars`:

```hcl
aws_profile  = "your-aws-profile"
project_name = "instaclone"
aws_region   = "ap-southeast-1"
environment  = "prod"
vpc_cidr     = "10.0.0.0/16"
db_password  = "your-secure-db-password"
jwt_key      = "your-base64-jwt-key-at-least-64-chars"
```

Generate a JWT key:

```bash
openssl rand -base64 64
```

### 2. Bootstrap remote state

First apply only the state resources:

```bash
terraform init

terraform apply \
  -target=aws_s3_bucket.terraform_state \
  -target=aws_s3_bucket_versioning.terraform_state \
  -target=aws_s3_bucket_server_side_encryption_configuration.terraform_state \
  -target=aws_s3_bucket_public_access_block.terraform_state \
  -target=aws_dynamodb_table.terraform_locks
```

Then uncomment the S3 backend in `providers.tf` and migrate:

```bash
terraform init -migrate-state \
  -backend-config="profile=your-aws-profile"
```

### 3. Deploy infrastructure

```bash
terraform plan -out=tfplan
terraform apply tfplan
```

This provisions the full stack (~10-15 minutes). Note the outputs:

```
cloudfront_domain_name = "dxxxxxxxxxx.cloudfront.net"
ecr_repository_url     = "123456789.dkr.ecr.ap-southeast-1.amazonaws.com/instaclone-prod-api"
```

### 4. Build and push API image

```bash
# Login to ECR
aws ecr get-login-password --region ap-southeast-1 --profile your-aws-profile \
  | docker login --username AWS --password-stdin 123456789.dkr.ecr.ap-southeast-1.amazonaws.com

# Build and push
cd backend/src/InstaClone.Api
docker build --platform linux/amd64 -t 123456789.dkr.ecr.ap-southeast-1.amazonaws.com/instaclone-prod-api:latest .
docker push 123456789.dkr.ecr.ap-southeast-1.amazonaws.com/instaclone-prod-api:latest
```

### 5. Deploy frontend

```bash
cd frontend
npm install && npm run build

aws s3 sync dist/ s3://instaclone-prod-frontend/ \
  --profile your-aws-profile --delete
```

### 6. Force ECS deployment

```bash
aws ecs update-service \
  --cluster instaclone-prod \
  --service instaclone-prod-api \
  --force-new-deployment \
  --region ap-southeast-1 \
  --profile your-aws-profile
```

### 7. Invalidate CloudFront cache

```bash
aws cloudfront create-invalidation \
  --distribution-id YOUR_DISTRIBUTION_ID \
  --paths "/*" \
  --profile your-aws-profile
```

The app is now live at the CloudFront URL from step 3.

## Estimated Cost

~$61/month for light traffic (Singapore region). Main costs: RDS ($22), ALB ($18), ECS Fargate ($18).

## Teardown

```bash
cd terraform
terraform destroy
```

Note: RDS has `skip_final_snapshot = false`, so a final snapshot will be created before deletion.
