variable "project_name" {
  description = "Project name used for resource naming"
  type        = string
  default     = "instaclone"
}

variable "aws_region" {
  description = "AWS region for all resources"
  type        = string
  default     = "us-east-1"
}

variable "environment" {
  description = "Environment name (e.g. prod, staging)"
  type        = string
  default     = "prod"
}

variable "vpc_cidr" {
  description = "CIDR block for the VPC"
  type        = string
  default     = "10.0.0.0/16"
}

variable "db_password" {
  description = "Password for the RDS database"
  type        = string
  sensitive   = true
}

variable "jwt_key" {
  description = "Secret key for JWT token signing"
  type        = string
  sensitive   = true
}

variable "domain_name" {
  description = "Custom domain name (optional, leave empty to skip DNS)"
  type        = string
  default     = ""
}
