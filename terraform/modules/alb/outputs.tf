output "alb_arn" {
  value = aws_lb.main.arn
}

output "alb_dns_name" {
  value = aws_lb.main.dns_name
}

output "target_group_arn" {
  value = aws_lb_target_group.api.arn
}

output "security_group_id" {
  value = aws_security_group.alb.id
}

output "listener_arn" {
  description = "ARN of the active listener (HTTPS if available, otherwise HTTP)"
  value       = var.certificate_arn != "" ? aws_lb_listener.https[0].arn : aws_lb_listener.http.arn
}
