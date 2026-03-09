# --- ALB restriction: only allow traffic from CloudFront ---

data "aws_ec2_managed_prefix_list" "cloudfront" {
  name = "com.amazonaws.global.cloudfront.origin-facing"
}

resource "aws_security_group_rule" "alb_cloudfront_http" {
  type              = "ingress"
  from_port         = 80
  to_port           = 80
  protocol          = "tcp"
  prefix_list_ids   = [data.aws_ec2_managed_prefix_list.cloudfront.id]
  security_group_id = module.alb.security_group_id
  description       = "HTTP from CloudFront only"
}

resource "aws_security_group_rule" "alb_cloudfront_https" {
  type              = "ingress"
  from_port         = 443
  to_port           = 443
  protocol          = "tcp"
  prefix_list_ids   = [data.aws_ec2_managed_prefix_list.cloudfront.id]
  security_group_id = module.alb.security_group_id
  description       = "HTTPS from CloudFront only"
}

# --- CloudWatch Alarm: unhealthy target count ---

resource "aws_cloudwatch_metric_alarm" "unhealthy_targets" {
  alarm_name          = "${var.project_name}-${var.environment}-unhealthy-targets"
  comparison_operator = "GreaterThanThreshold"
  evaluation_periods  = 2
  metric_name         = "UnHealthyHostCount"
  namespace           = "AWS/ApplicationELB"
  period              = 60
  statistic           = "Maximum"
  threshold           = 0
  alarm_description   = "ECS task is unhealthy"
  treat_missing_data  = "breaching"

  dimensions = {
    TargetGroup  = module.alb.target_group_arn_suffix
    LoadBalancer = module.alb.alb_arn_suffix
  }
}
