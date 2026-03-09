namespace InstaClone.Api.DTOs;

public class ErrorResponse
{
    public string Message { get; set; } = default!;
    public List<string> Errors { get; set; } = new();
}
