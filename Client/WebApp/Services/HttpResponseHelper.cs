namespace WebApp.Services;

public class HttpResponseHelper
{
    public static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(error, null, response.StatusCode);
        }
    }
}