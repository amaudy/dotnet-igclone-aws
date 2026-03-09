# --- ALB ingress: allow HTTP/HTTPS from anywhere ---
# The CloudFront managed prefix list exceeds the 60-rule SG limit.
# For Phase 1, allow public access. Phase 2 will add WAF on CloudFront
# for proper traffic filtering (rate limiting, bot protection, etc).

resource "aws_security_group_rule" "alb_http" {
  type              = "ingress"
  from_port         = 80
  to_port           = 80
  protocol          = "tcp"
  cidr_blocks       = ["0.0.0.0/0"]
  security_group_id = module.alb.security_group_id
  description       = "HTTP from anywhere"
}

resource "aws_security_group_rule" "alb_https" {
  type              = "ingress"
  from_port         = 443
  to_port           = 443
  protocol          = "tcp"
  cidr_blocks       = ["0.0.0.0/0"]
  security_group_id = module.alb.security_group_id
  description       = "HTTPS from anywhere"
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
