# Phase 1 — Core Infrastructure (Complete)

## Stage 2: Networking + ECR
**Status**: Complete
- VPC with 2 public + 2 private subnets across 2 AZs
- ECR repository with lifecycle policy (keep last 5)
- ECS runs in public subnets (no VPC endpoints needed)

## Stage 3: Database + Secrets + ALB + ECS
**Status**: Complete
- RDS SQL Server Express (db.t3.micro, single-AZ, final snapshot enabled)
- SSM Parameter Store for db_password + jwt_key
- ALB with health check target group, restricted to CloudFront prefix list
- ECS Fargate (0.25 vCPU, 512MB) with deployment circuit breaker
- Zero-downtime deploys (min_healthy=100%, max=200%)

## Stage 4: S3 + CloudFront
**Status**: Complete
- S3 buckets for frontend (SPA) + uploads (images)
- CloudFront with 3 behaviors: `/*` → S3, `/api/*` → ALB, `/uploads/*` → S3
- OAC for secure S3 access

## Stage 5: DNS + SSL + Hardening
**Status**: Complete
- ACM certificate + Route53 (conditional on domain_name)
- ALB restricted to CloudFront managed prefix list
- CloudWatch alarm on unhealthy targets
- S3 remote state with DynamoDB locking

---

# Phase 2 — Production Hardening (Future)

## WAF
- AWS WAF on CloudFront distribution
- Managed rule groups: AWSManagedRulesCommonRuleSet, AWSManagedRulesSQLiRuleSet
- Rate limiting rule
- Estimated cost: ~$6/mo + $0.60/million requests

## CI/CD Pipeline
- GitHub Actions workflow: build → push ECR → update ECS service
- Frontend: build → sync S3 → invalidate CloudFront
- Separate migration step before ECS deploy (RunTask)

## CloudWatch Observability
- Alarms: 5xx error rate, p99 latency, CPU/memory utilization, RDS connections
- Dashboard for key metrics
- Structured logging

## RDS Multi-AZ
- Enable `multi_az = true` for automatic failover
- Estimated additional cost: ~$14/mo

## Auto-scaling
- ECS service auto-scaling (target tracking on CPU/memory)
- Minimum 2 tasks across 2 AZs for high availability
