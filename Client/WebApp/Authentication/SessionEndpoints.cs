using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DTO.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Authentication;

public static class SessionEndpoints
{
    public static void MapSessionEndpoints(this WebApplication app)
    {
        app.MapPost("/session/login", async (
            [FromForm] string username,
            [FromForm] string password,
            HttpContext context,
            IHttpClientFactory clientFactory) =>
        {
            var client = clientFactory.CreateClient("api");

            var response = await client.PostAsJsonAsync(
                "auth/login",
                new LoginRequest(username, password));

            if (!response.IsSuccessStatusCode)
                return Results.Redirect("/login?error=invalid");

            var result = await response.Content
                .ReadFromJsonAsync<AuthResponse>();

            if (result is null)
                return Results.Redirect("/login?error=server");

            await SignInAsync(context, result.Token);

            return Results.Redirect("/");
        });

        app.MapPost("/session/register", async (
            [FromForm] string username,
            [FromForm] string password,
            [FromForm] string confirmPassword,
            HttpContext context,
            IHttpClientFactory clientFactory) =>
        {
            if (password != confirmPassword)
                return Results.Redirect("/register?error=mismatch");

            var client = clientFactory.CreateClient("api");

            var response = await client.PostAsJsonAsync(
                "auth/register",
                new RegisterRequest(username, password));

            if (!response.IsSuccessStatusCode)
            {
                var error = response.StatusCode ==
                            System.Net.HttpStatusCode.Conflict
                    ? "taken"
                    : "server";

                return Results.Redirect($"/register?error={error}");
            }

            var result = await response.Content
                .ReadFromJsonAsync<AuthResponse>();

            if (result is null)
                return Results.Redirect("/register?error=server");

            await SignInAsync(context, result.Token);

            return Results.Redirect("/");
        });

        app.MapPost("/session/logout", async (HttpContext context) =>
        {
            await context.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return Results.Redirect("/");
        });
        
        
        app.MapPost("/session/delete", async (
            [FromForm] string confirmation,
            HttpContext context,
            IHttpClientFactory clientFactory) =>
        {
            if (context.User.Identity?.IsAuthenticated != true)
            {
                return Results.Redirect("/login");
            }

            if (confirmation != "DELETE")
            {
                return Results.Redirect("/settings?error=confirmation");
            }

            var token = context.User.FindFirst("api_token")?.Value;

            if (string.IsNullOrWhiteSpace(token))
            {
                return Results.Redirect("/settings?error=session");
            }

            var client = clientFactory.CreateClient("api");

            using var request = new HttpRequestMessage(
                HttpMethod.Delete,
                "users/me"
            );

            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer",
                    token
                );

            using var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return Results.Redirect("/settings?error=delete");
            }

            await context.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            return Results.Redirect("/");
        }).RequireAuthorization();

    }

    private static async Task SignInAsync(
        HttpContext context,
        string token)
    {
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        var userId = jwt.Subject;
        var username = jwt.Claims
            .FirstOrDefault(c => c.Type == "unique_name")?.Value;

        if (string.IsNullOrWhiteSpace(userId) ||
            string.IsNullOrWhiteSpace(username) ||
            jwt.ValidTo <= DateTime.UtcNow)
        {
            throw new InvalidOperationException(
                "The authentication API returned an invalid token.");
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Name, username),
            new("api_token", token)
        };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        await context.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = new DateTimeOffset(jwt.ValidTo)
            });
    }
}