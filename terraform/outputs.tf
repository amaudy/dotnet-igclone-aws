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
