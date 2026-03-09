terraform {
  required_version = ">= 1.5"

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }

  # Step 1: Apply with -target to create the S3 bucket and DynamoDB table (see state.tf)
  # Step 2: Uncomment the block below
  # Step 3: Run: terraform init -migrate-state
  backend "s3" {
    bucket         = "instaclone-terraform-state"
    key            = "terraform.tfstate"
    region         = "ap-southeast-1"
    dynamodb_table = "instaclone-terraform-locks"
    encrypt        = true
  }
}

provider "aws" {
  region  = var.aws_region
  profile = var.aws_profile

  default_tags {
    tags = {
      Project     = var.project_name
      Environment = var.environment
      ManagedBy   = "terraform"
    }
  }
}

# CloudFront requires ACM certificates in us-east-1
provider "aws" {
  alias   = "us_east_1"
  region  = "us-east-1"
  profile = var.aws_profile

  default_tags {
    tags = {
      Project     = var.project_name
      Environment = var.environment
      ManagedBy   = "terraform"
    }
  }
}
