output "db_password_arn" {
  value = aws_ssm_parameter.db_password.arn
}

output "jwt_key_arn" {
  value = aws_ssm_parameter.jwt_key.arn
}
