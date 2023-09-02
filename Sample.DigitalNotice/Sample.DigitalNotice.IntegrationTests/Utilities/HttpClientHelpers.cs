using System.Text.Json;
using System.Text;

namespace Sample.DigitalNotice.IntegrationTests.Utilities;

internal static class HttpClientHelpers
{
    private static readonly JsonSerializerOptions serializerOptions;

    static HttpClientHelpers()
    {
        serializerOptions = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }

    internal static async Task<TResponse> SendRequestAsync<TResponse>(this HttpClient client, HttpMethod method, string url, object requestBody = null)
    {
        var response = await client.SendRequestAsync(method, url, requestBody);

        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<TResponse>(responseContent, serializerOptions);

        return result;
    }

    internal static async Task<HttpResponseMessage> SendRequestAsync(this HttpClient client, HttpMethod method, string url, object requestBody = null)
    {
        using var request = new HttpRequestMessage(method, url);

        if (requestBody is not null)
        {
            request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        }

        return await client.SendAsync(request);
    }
}
