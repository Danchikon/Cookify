using Cookify.Application.Models;
using Cookify.Application.Services;
using Cookify.Infrastructure.Options;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cookify.Infrastructure.Services;

public class GoogleFileStorageService : IFileStorageService
{
    private static readonly SemaphoreSlim SemaphoreSlim = new(5);
    
    private readonly GoogleStorageOptions _options;
    private readonly Lazy<StorageClient> _storageClient;
    private readonly ILogger<GoogleFileStorageService> _logger;

    public GoogleFileStorageService(
        Lazy<StorageClient> storageClient, 
        IOptions<GoogleStorageOptions> options,
        ILogger<GoogleFileStorageService> logger
        )
    {
        _storageClient = storageClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<string> PutFileAsync(FileModel file, CancellationToken cancellationToken)
    {
        try
        {
            await SemaphoreSlim.WaitAsync(cancellationToken);
            
            _logger.LogInformation("Putting {FileName} file into google storage", file.Name);
            
            var uploadedFile = await _storageClient.Value.UploadObjectAsync(
                _options.Bucket,
                file.Name,
                file.ContentType,
                file.Stream, 
                cancellationToken: cancellationToken
                );

            _logger.LogInformation("File {FileName} putted into google storage", file.Name);
            
            
            return GetFileLink(uploadedFile.Name);
        }
        finally
        {
            SemaphoreSlim.Release();
        }
    }

    public string GetFileLink(string fileName)
    {
        return $"{_options.Url}/{_options.Bucket}/{fileName}";
    }

    public async Task<FileModel> GetFileAsync(string fileName, CancellationToken cancellationToken)
    {
        var stream = new MemoryStream();
        var file = await _storageClient.Value.DownloadObjectAsync(_options.Bucket, fileName, stream, cancellationToken: cancellationToken);
        
        return new FileModel(stream, file.ContentType, file.Name);
    }

    public async Task RemoveFileAsync(string fileName, CancellationToken cancellationToken)
    {
        await _storageClient.Value.DeleteObjectAsync(_options.Bucket, fileName, cancellationToken: cancellationToken);
    }
}