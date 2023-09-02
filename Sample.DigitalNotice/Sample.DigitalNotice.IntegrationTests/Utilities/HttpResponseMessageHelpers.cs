using System.Text.Json;

namespace Sample.DigitalNotice.IntegrationTests.Utilities;

internal static class HttpResponseMessageHelpers
{
    private static readonly JsonSerializerOptions serializerOptions;

    static HttpResponseMessageHelpers()
    {
        serializerOptions = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }

    internal static async Task<T> DeserializeContentAsync<T>(this HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<T>(content, serializerOptions);
    }
}
