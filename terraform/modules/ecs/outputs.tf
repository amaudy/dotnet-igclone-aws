output "cluster_name" {
  value = aws_ecs_cluster.main.name
}

output "service_name" {
  value = aws_ecs_service.api.name
}

output "task_execution_role_arn" {
  value = aws_iam_role.task_execution.arn
}
