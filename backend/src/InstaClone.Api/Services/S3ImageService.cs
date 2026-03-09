using Amazon.S3;
using Amazon.S3.Model;

namespace InstaClone.Api.Services;

public class S3ImageService : IImageService
{
    private readonly IAmazonS3 _s3;
    private readonly string _bucketName;
    private readonly string _cdnBaseUrl;

    public S3ImageService(IAmazonS3 s3, IConfiguration config)
    {
        _s3 = s3;
        _bucketName = config["Aws:S3:BucketName"]
            ?? throw new InvalidOperationException("Aws:S3:BucketName is not configured");
        _cdnBaseUrl = config["Aws:S3:CdnBaseUrl"]?.TrimEnd('/')
            ?? throw new InvalidOperationException("Aws:S3:CdnBaseUrl is not configured");
    }

    public async Task<string> SaveImageAsync(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        var key = $"uploads/{Guid.NewGuid()}{extension}";

        using var stream = file.OpenReadStream();
        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = stream,
            ContentType = file.ContentType
        };

        await _s3.PutObjectAsync(request);

        return $"{_cdnBaseUrl}/{key}";
    }

    public void DeleteImage(string imageUrl)
    {
        var uri = new Uri(imageUrl);
        var key = uri.AbsolutePath.TrimStart('/');

        _s3.DeleteObjectAsync(new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        }).GetAwaiter().GetResult();
    }
}
