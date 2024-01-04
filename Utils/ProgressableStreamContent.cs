using System.IO;
using System.Net;
using System.Net.Http;

namespace TestBuilder.Utils;

public class ProgressableStreamContent : HttpContent
{
    private const int DefaultBufferSize = 4096;
    private readonly HttpContent _content;
    private readonly int _bufferSize;
    private readonly Action<long, long> _progress;

    public ProgressableStreamContent(HttpContent content, Action<long, long> progress)
    {
        ArgumentNullException.ThrowIfNull(content);
        ArgumentNullException.ThrowIfNull(progress);

        _content = content;
        _bufferSize = DefaultBufferSize;
        _progress = progress;

        foreach (var header in content.Headers)
        {
            Headers.Add(header.Key, header.Value);
        }
    }

    protected override async Task SerializeToStreamAsync(Stream stream, TransportContext? context)
    {
        var buffer = new byte[_bufferSize];
        TryComputeLength(out _);
        var uploaded = 0;

        await using var contentStream = await _content.ReadAsStreamAsync();
        var length = contentStream.Length;
        int read;
            
        while ((read = await contentStream.ReadAsync(buffer)) > 0)
        {
            await stream.WriteAsync(buffer.AsMemory(0, read));
            uploaded += read;
            _progress(uploaded, length);
        }
    }

    protected override bool TryComputeLength(out long length)
    {
        length = _content.Headers.ContentLength.GetValueOrDefault();
        return true;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _content.Dispose();
        }
        base.Dispose(disposing);
    }
}