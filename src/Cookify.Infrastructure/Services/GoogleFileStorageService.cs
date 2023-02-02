using Cookify.Application.Models;
using Cookify.Application.Services;
using Cookify.Infrastructure.Options;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cookify.Infrastructure.Services;

public class GoogleFileStorageService : IFileStorageService
{
    private readonly GoogleStorageOptions _options;
    private readonly StorageClient _storageClient;

    public GoogleFileStorageService(StorageClient storageClient, IOptions<GoogleStorageOptions> options)
    {
        _storageClient = storageClient;
        _options = options.Value;
    }

    public async Task<string> PutFileAsync(FileModel file)
    {
        var uploadedFile = await _storageClient.UploadObjectAsync(
            _options.Bucket,
            file.Name,
            file.ContentType,
            file.Stream
        );

        return $"{_options.Url}/{_options.Bucket}/{uploadedFile.Name}";
    }

    public string GetFileLink(string fileName)
    {
        return $"{_options.Url}/{_options.Bucket}/{fileName}";
    }
    
    public async Task<FileModel> GetFileAsync(string fileName)
    {
        var stream = new MemoryStream();
        var file = await _storageClient.DownloadObjectAsync(_options.Bucket, fileName, stream);

        return new FileModel(stream, file.ContentType, file.Name);
    }

    public async Task RemoveFileAsync(string fileName)
    {
        await _storageClient.DeleteObjectAsync(_options.Bucket, fileName);
    }
}