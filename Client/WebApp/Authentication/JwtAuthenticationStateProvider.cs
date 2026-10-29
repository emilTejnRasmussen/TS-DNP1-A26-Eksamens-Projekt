using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace WebApp.Authentication;

public class JwtAuthenticationStateProvider(TokenStore tokenStore) : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var principal = CreatePrincipal(tokenStore.Token);

        return Task.FromResult(new AuthenticationState(principal));
    }

    public void SignIn(string token)
    {
        var principal = CreatePrincipal(token);

        if (principal.Identity?.IsAuthenticated != true)
            throw new InvalidOperationException("Invalid or expired JWT.");

        tokenStore.SetToken(token);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    public void SignOut()
    {
        tokenStore.Clear();

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private static ClaimsPrincipal CreatePrincipal(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return Anonymous();

        try
        {
            var parts = token.Split('.');

            if (parts.Length != 3)
                return Anonymous();

            var payload = parts[1]
                .Replace('-', '+')
                .Replace('_', '/');

            payload = payload.PadRight(
                payload.Length + (4 - payload.Length % 4) % 4,
                '='
            );

            var json = Encoding.UTF8.GetString(
                Convert.FromBase64String(payload)
            );

            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;

            var userId = root.GetProperty("sub").GetString();
            var username = root.GetProperty("unique_name").GetString();
            var expires = root.GetProperty("exp").GetInt64();

            if (string.IsNullOrWhiteSpace(userId) ||
                expires <= DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            {
                return Anonymous();
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId)
            };

            if (!string.IsNullOrWhiteSpace(username))
                claims.Add(new Claim(ClaimTypes.Name, username));

            var identity = new ClaimsIdentity(claims, "Jwt");

            return new ClaimsPrincipal(identity);
        }
        catch (Exception ex) when (
            ex is JsonException or
                FormatException or
                InvalidOperationException or
                ArgumentException or
                OverflowException)
        {
            return Anonymous();
        }
    }

    private static ClaimsPrincipal Anonymous()
    {
        return new ClaimsPrincipal(new ClaimsIdentity());
    }
}