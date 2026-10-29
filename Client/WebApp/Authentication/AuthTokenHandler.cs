using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;

namespace WebApp.Authentication;

public class AuthTokenHandler(AuthenticationStateProvider authenticationStateProvider) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var state = await authenticationStateProvider.GetAuthenticationStateAsync();

        var user = state.User;

        if (user.Identity?.IsAuthenticated == true)
        {
            var token = user.FindFirst("api_token")?.Value;

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}