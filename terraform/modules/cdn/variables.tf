variable "project_name" {
  type = string
}

variable "environment" {
  type = string
}

variable "frontend_bucket_regional_domain_name" {
  type = string
}

variable "frontend_bucket_id" {
  type = string
}

variable "uploads_bucket_regional_domain_name" {
  type = string
}

variable "uploads_bucket_id" {
  type = string
}

variable "alb_dns_name" {
  type = string
}

variable "frontend_bucket_arn" {
  type = string
}

variable "uploads_bucket_arn" {
  type = string
}

variable "certificate_arn" {
  description = "ACM certificate ARN (optional, for custom domain)"
  type        = string
  default     = ""
}

variable "domain_name" {
  description = "Custom domain name (optional)"
  type        = string
  default     = ""
}
