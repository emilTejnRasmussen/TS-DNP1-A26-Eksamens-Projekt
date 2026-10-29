using DTO.Auth;
using WebApp.Authentication;
using WebApp.Services.Interfaces;

namespace WebApp.Services.HttpImpl;

public class HttpAuthService(
    HttpClient client,
    JwtAuthenticationStateProvider authStateProvider) : IAuthService
{
    public async Task LoginAsync(LoginRequest request)
    {
        var response = await client.PostAsJsonAsync("auth/login", request);

        await HttpResponseHelper.EnsureSuccessAsync(response);

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>()
                     ?? throw new InvalidOperationException("Invalid login response.");

        authStateProvider.SignIn(result.Token);
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        var response = await client.PostAsJsonAsync("auth/register", request);

        await HttpResponseHelper.EnsureSuccessAsync(response);

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>()
                     ?? throw new InvalidOperationException("Invalid registration response.");

        authStateProvider.SignIn(result.Token);
    }

    public void Logout()
    {
        authStateProvider.SignOut();
    }
}