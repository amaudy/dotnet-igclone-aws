from diagrams import Diagram, Cluster, Edge
from diagrams.aws.network import CloudFront, ALB, VPC, PublicSubnet, PrivateSubnet
from diagrams.aws.compute import ECS, Fargate
from diagrams.aws.database import RDS
from diagrams.aws.storage import S3
from diagrams.aws.security import ACM
from diagrams.aws.management import Cloudwatch, SystemsManager
from diagrams.aws.engagement import Connect
from diagrams.onprem.client import Users

graph_attr = {
    "fontsize": "14",
    "bgcolor": "white",
    "pad": "0.5",
    "nodesep": "0.8",
    "ranksep": "1.2",
}

with Diagram(
    "InstaClone - AWS Architecture",
    filename="instaclone_aws_architecture",
    show=False,
    direction="TB",
    graph_attr=graph_attr,
    outformat="png",
):
    users = Users("Users")

    cloudfront = CloudFront("CloudFront\nCDN")

    s3_frontend = S3("S3\nFrontend\n(Vue.js SPA)")
    s3_uploads = S3("S3\nUploads\n(Images)")

    with Cluster("VPC - 10.0.0.0/16\nap-southeast-1"):
        with Cluster("Public Subnets (2 AZs)"):
            alb = ALB("ALB\n(/api/*)")

            with Cluster("ECS Fargate"):
                ecs = Fargate(".NET 8 API\n0.5 vCPU / 1GB")

        with Cluster("Private Subnets (2 AZs)"):
            rds = RDS("RDS SQL Server\nExpress\ndb.t3.micro")

    ssm = SystemsManager("SSM\nParameter Store\n(Secrets)")
    cloudwatch = Cloudwatch("CloudWatch\nLogs")

    # User flow
    users >> Edge(label="HTTPS") >> cloudfront

    # CloudFront behaviors
    cloudfront >> Edge(label="/*", color="blue") >> s3_frontend
    cloudfront >> Edge(label="/api/*", color="green") >> alb
    cloudfront >> Edge(label="/uploads/*", color="orange") >> s3_uploads

    # ALB to ECS
    alb >> Edge(label=":8080") >> ecs

    # ECS connections
    ecs >> Edge(label="SQL 1433") >> rds
    ecs >> Edge(label="PutObject", color="orange") >> s3_uploads
    ecs >> Edge(style="dashed", color="gray") >> ssm
    ecs >> Edge(style="dashed", color="gray") >> cloudwatch
