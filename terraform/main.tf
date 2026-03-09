module "networking" {
  source = "./modules/networking"

  project_name = var.project_name
  environment  = var.environment
  vpc_cidr     = var.vpc_cidr
}

module "ecr" {
  source = "./modules/ecr"

  project_name = var.project_name
  environment  = var.environment
}

module "secrets" {
  source = "./modules/secrets"

  project_name = var.project_name
  environment  = var.environment
  db_password  = var.db_password
  jwt_key      = var.jwt_key
}

module "storage" {
  source = "./modules/storage"

  project_name = var.project_name
  environment  = var.environment
}

module "cdn" {
  source = "./modules/cdn"

  project_name                         = var.project_name
  environment                          = var.environment
  frontend_bucket_regional_domain_name = module.storage.frontend_bucket_regional_domain_name
  frontend_bucket_id                   = module.storage.frontend_bucket_id
  frontend_bucket_arn                  = module.storage.frontend_bucket_arn
  uploads_bucket_regional_domain_name  = module.storage.uploads_bucket_regional_domain_name
  uploads_bucket_id                    = module.storage.uploads_bucket_id
  uploads_bucket_arn                   = module.storage.uploads_bucket_arn
  alb_dns_name                         = module.alb.alb_dns_name
}

module "alb" {
  source = "./modules/alb"

  project_name      = var.project_name
  environment       = var.environment
  vpc_id            = module.networking.vpc_id
  public_subnet_ids = module.networking.public_subnet_ids
}

# ECS security group — created at root level to break circular dependency
# between ECS (needs db_host) and database (needs ecs_security_group_id)
resource "aws_security_group" "ecs" {
  name_prefix = "${var.project_name}-${var.environment}-ecs-"
  description = "Security group for ECS tasks"
  vpc_id      = module.networking.vpc_id

  ingress {
    description     = "HTTP from ALB"
    from_port       = 8080
    to_port         = 8080
    protocol        = "tcp"
    security_groups = [module.alb.security_group_id]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Name = "${var.project_name}-${var.environment}-ecs-sg"
  }
}

module "database" {
  source = "./modules/database"

  project_name          = var.project_name
  environment           = var.environment
  vpc_id                = module.networking.vpc_id
  private_subnet_ids    = module.networking.private_subnet_ids
  ecs_security_group_id = aws_security_group.ecs.id
  db_password           = var.db_password
}

module "ecs" {
  source = "./modules/ecs"

  project_name          = var.project_name
  environment           = var.environment
  aws_region            = var.aws_region
  vpc_id                = module.networking.vpc_id
  private_subnet_ids    = module.networking.private_subnet_ids
  target_group_arn      = module.alb.target_group_arn
  ecr_repository_url    = module.ecr.repository_url
  db_host               = module.database.endpoint
  db_password_arn       = module.secrets.db_password_arn
  jwt_key_arn           = module.secrets.jwt_key_arn
  uploads_bucket_name   = module.storage.uploads_bucket_name
  uploads_bucket_arn    = module.storage.uploads_bucket_arn
  cdn_base_url          = "https://${module.cdn.distribution_domain_name}"
  cors_origin           = "https://${module.cdn.distribution_domain_name}"
  alb_security_group_id = module.alb.security_group_id
  ecs_security_group_id = aws_security_group.ecs.id
}
