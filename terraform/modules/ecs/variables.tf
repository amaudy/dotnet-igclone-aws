variable "project_name" {
  type = string
}

variable "environment" {
  type = string
}

variable "aws_region" {
  type = string
}

variable "vpc_id" {
  type = string
}

variable "private_subnet_ids" {
  type = list(string)
}

variable "target_group_arn" {
  type = string
}

variable "ecr_repository_url" {
  type = string
}

variable "image_tag" {
  description = "Docker image tag to deploy"
  type        = string
  default     = "latest"
}

variable "db_host" {
  type = string
}

variable "db_name" {
  type    = string
  default = "InstaClone"
}

variable "db_password_arn" {
  description = "SSM parameter ARN for database password"
  type        = string
}

variable "jwt_key_arn" {
  description = "SSM parameter ARN for JWT signing key"
  type        = string
}

variable "uploads_bucket_name" {
  description = "S3 bucket name for image uploads"
  type        = string
}

variable "uploads_bucket_arn" {
  description = "S3 bucket ARN for image uploads"
  type        = string
}

variable "cdn_base_url" {
  description = "CloudFront base URL for serving uploads"
  type        = string
}

variable "cors_origin" {
  description = "Allowed CORS origin (CloudFront domain)"
  type        = string
}

variable "alb_security_group_id" {
  type = string
}

variable "cpu" {
  type    = number
  default = 256
}

variable "memory" {
  type    = number
  default = 512
}
