namespace Royal.Service.Utilities;

internal sealed class HttpMethods
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async static Task<(T, HttpStatusCode)> ResolveResponse<T, TService>(
        HttpResponseMessage response,
        ILogger<TService> logger,
        string usedServiceMethod)
    {
        logger.LogInformation($"[Service = {nameof(TService)}] [Method = {usedServiceMethod}] Initializing deserialization of content");

        var content = await response.Content.ReadAsStringAsync();

        var item = JsonSerializer.Deserialize<T>(content, jsonSerializerOptions);

        logger.LogInformation($"[Service = {nameof(TService)}] [Method = {usedServiceMethod}] Content deserialized, {nameof(item)} found");

        return (item, response.StatusCode);
    }
}
