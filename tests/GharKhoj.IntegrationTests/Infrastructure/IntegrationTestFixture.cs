using System.Net.Http.Headers;
using System.Net.Http.Json;
using Docker.DotNet.Models;
using GharKhoj.Api.Models.Authentication;
using GharKhoj.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GharKhoj.IntegrationTests.Infrastructure;

public abstract class IntegrationTestFixture : IClassFixture<GharKhojWebAppFactory>
{
    private readonly GharKhojWebAppFactory _factory;

    protected IntegrationTestFixture(GharKhojWebAppFactory factory)
    {
        _factory = factory;
    }

    private HttpClient? _authorizedClient;

    public HttpClient CreateClient() => _factory.CreateClient();

    public async Task<HttpClient> CreateAuthorizedClientAsync(
        string email = "contact@saimankhatiwada.com.np", 
        string password = "9d9dfbfbfdfd@S", 
        bool forceNewClient = false)
    {
        if (_authorizedClient is not null && !forceNewClient)
        {
            return _authorizedClient;
        }

        HttpClient client = CreateClient();

        bool userExists;

        using IServiceScope scope = _factory.Services.CreateScope();

        using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        userExists = await dbContext.Users.AnyAsync(u => u.Email == email);

        if (!userExists)
        {
            HttpResponseMessage registerResponse = await client.PostAsJsonAsync(Routes.Auth.Register,
                new RegisterUserDto
                {
                    FirstName = "Saiman",
                    LastName = "Khatiwada",
                    Email = email,
                    Password = password,
                    Role = "SuperAdmin"
                });

            registerResponse.EnsureSuccessStatusCode();
        }

        HttpResponseMessage loginResponse = await client.PostAsJsonAsync(Routes.Auth.Login,
            new LoginUserDto
            {
                Email = email,
                Password = password
            });

        loginResponse.EnsureSuccessStatusCode();

        AuthorizationTokenDto? loginResult = await loginResponse.Content.ReadFromJsonAsync<AuthorizationTokenDto>();

        if (loginResult?.AccessToken is null)
        {
            throw new InvalidOperationException("Failed to get authorization token");
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.AccessToken);

        if (!forceNewClient)
        {
            _authorizedClient = client;
        }

        return client;
    }
}
