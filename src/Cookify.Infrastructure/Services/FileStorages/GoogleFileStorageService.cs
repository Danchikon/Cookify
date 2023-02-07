using Cookify.Application.Models;
using Cookify.Application.Services;
using Cookify.Infrastructure.Options;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cookify.Infrastructure.Services.FileStorages;

public class GoogleFileStorageService : IFileStorageService
{
    private static readonly object LockObject = new();
    
    private static Lazy<SemaphoreSlim> _semaphoreSlim = new();
    
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
        
        lock (LockObject)
        {
            if (_semaphoreSlim.IsValueCreated)
            {
                return;
            }
            
            _semaphoreSlim = new Lazy<SemaphoreSlim>(() => new SemaphoreSlim(_options.MaximumConcurrency));
            var _ = _semaphoreSlim.Value;
        }
    }

    public async Task<string> PutFileAsync(FileModel file, CancellationToken cancellationToken)
    {
        try
        {
            await _semaphoreSlim.Value.WaitAsync(cancellationToken);
            
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
            _semaphoreSlim.Value.Release();
        }
    }

    public string GetFileLink(string fileName)
    {
        return $"{_options.Url}/{_options.Bucket}/{fileName}";
    }

    public async Task RemoveFileAsync(string fileName, CancellationToken cancellationToken)
    {
        try
        {
            await _semaphoreSlim.Value.WaitAsync(cancellationToken);
            
            _logger.LogInformation("Removing {FileName} file from google storage", fileName);
            
            await _storageClient.Value.DeleteObjectAsync(_options.Bucket, fileName, cancellationToken: cancellationToken);
            
            _logger.LogInformation("File {FileName} removed from google storage", fileName);
        }
        finally
        {
            _semaphoreSlim.Value.Release();
        }
    }
}