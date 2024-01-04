using System.IO;
using System.Net.Http;
using System.Text.Json;

namespace TestBuilder.Utils;

public abstract class Image
{
    private static readonly HttpClient Client = new();

    private const string ApiKey =
        "chv_Pa8_344c84f124ae1c2767a944a6bdc831d87c5cd9349399b8d8734ea51da522b78b7e471257214fe6bcd1dc26d82793362d5ca99279af1ac21c5277bcb3f0f8c86a";

    public static async Task<string> UploadImage(string filePath, IProgress<(long uploaded, long total)> progress)
    {
        var imageBytes = await File.ReadAllBytesAsync(filePath);
        if (imageBytes.Length == 0) throw new Exception("No file was provided.");

        var content = new MultipartFormDataContent();
        var imageContent = new ByteArrayContent(imageBytes);
        content.Add(imageContent, "source", $"image{Random.Shared.Next(1, 5000)}");

        using var progressContent =
            new ProgressableStreamContent(content, (uploaded, total) => { progress.Report((uploaded, total)); });
        var response =
            await Client.PostAsync($"https://freeimghost.net/api/1/upload/?key={ApiKey}", progressContent);
        var json = await ResponseToJson(response);

        if (json.TryGetProperty("image", out var image))
            return image.GetProperty("url").GetString()!;

        throw new Exception(json.GetProperty("error").GetProperty("message").GetString()!);
    }

    private static async Task<JsonElement> ResponseToJson(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseJson = JsonDocument.Parse(responseContent).RootElement;
        return responseJson;
    }
}