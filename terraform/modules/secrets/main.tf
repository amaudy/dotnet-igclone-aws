resource "aws_ssm_parameter" "db_password" {
  name  = "/${var.project_name}/${var.environment}/db-password"
  type  = "SecureString"
  value = var.db_password

  tags = {
    Name = "${var.project_name}-${var.environment}-db-password"
  }
}

resource "aws_ssm_parameter" "jwt_key" {
  name  = "/${var.project_name}/${var.environment}/jwt-key"
  type  = "SecureString"
  value = var.jwt_key

  tags = {
    Name = "${var.project_name}-${var.environment}-jwt-key"
  }
}
