using System.Net;
using System.Net.Http.Json;
using InstaClone.Api.DTOs;
using Xunit;

namespace InstaClone.Tests;

public class AuthEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_WithValidData_ReturnsToken()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/register", new
        {
            username = "newuser",
            email = "new@example.com",
            password = "Test123@"
        });

        response.EnsureSuccessStatusCode();
        var token = await response.Content.ReadFromJsonAsync<TokenResponse>();
        Assert.NotNull(token);
        Assert.NotEmpty(token.Token);
    }

    [Fact]
    public async Task Register_WithInvalidData_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/register", new
        {
            username = "",
            email = "invalid",
            password = "x"
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsToken()
    {
        var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", new
        {
            username = "logintest",
            email = "login@example.com",
            password = "Test123@"
        });
        registerResponse.EnsureSuccessStatusCode();

        var response = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            email = "login@example.com",
            password = "Test123@"
        });

        response.EnsureSuccessStatusCode();
        var token = await response.Content.ReadFromJsonAsync<TokenResponse>();
        Assert.NotNull(token);
        Assert.NotEmpty(token.Token);
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ReturnsUnauthorized()
    {
        await _client.PostAsJsonAsync("/api/auth/register", new
        {
            username = "wrongpass",
            email = "wrong@example.com",
            password = "Test123@"
        });

        var response = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            email = "wrong@example.com",
            password = "WrongPassword1@"
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
