namespace InstaClone.Api.Services;

public interface IImageService
{
    Task<string> SaveImageAsync(IFormFile file);
    void DeleteImage(string imageUrl);
}

public class LocalImageService : IImageService
{
    private readonly string _uploadsPath;
    private readonly string _baseUrl;

    public LocalImageService(IWebHostEnvironment env, IConfiguration config)
    {
        _uploadsPath = Path.Combine(env.ContentRootPath, "uploads");
        Directory.CreateDirectory(_uploadsPath);
        _baseUrl = "/uploads";
    }

    public async Task<string> SaveImageAsync(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(_uploadsPath, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"{_baseUrl}/{fileName}";
    }

    public void DeleteImage(string imageUrl)
    {
        var fileName = Path.GetFileName(imageUrl);
        var filePath = Path.Combine(_uploadsPath, fileName);
        if (File.Exists(filePath))
            File.Delete(filePath);
    }
}
