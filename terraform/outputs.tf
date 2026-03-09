output "ecr_repository_url" {
  description = "ECR repository URL for the API image"
  value       = module.ecr.repository_url
}

output "alb_dns_name" {
  description = "ALB DNS name for the API"
  value       = module.alb.alb_dns_name
}

output "rds_endpoint" {
  description = "RDS database endpoint"
  value       = module.database.endpoint
}

output "cloudfront_domain_name" {
  description = "CloudFront distribution domain name"
  value       = module.cdn.distribution_domain_name
}

output "cloudfront_distribution_id" {
  description = "CloudFront distribution ID (for cache invalidation)"
  value       = module.cdn.distribution_id
}

output "frontend_bucket_id" {
  description = "S3 bucket for frontend static files"
  value       = module.storage.frontend_bucket_id
}
