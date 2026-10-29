using System.Net.Http.Headers;

namespace WebApp.Authentication;

public class AuthTokenHandler(TokenStore tokenStore) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(tokenStore.Token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", tokenStore.Token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}