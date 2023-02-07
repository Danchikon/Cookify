using Cookify.Application.Models;
using Cookify.Application.Services;
using Cookify.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;

namespace Cookify.Infrastructure.Services.FileStorages;

public class MinioFileStorageService : IFileStorageService
{
    private readonly ILogger<MinioFileStorageService> _logger;
    private readonly MinioStorageOptions _options;
    private readonly IMinioClient _minioClient;

    public MinioFileStorageService(
        ILogger<MinioFileStorageService> logger,
        IOptions<MinioStorageOptions> options,
        IMinioClient minioClient
        )
    {
        _logger = logger;
        _options = options.Value;
        _minioClient = minioClient;
    }
    
    public async Task<string> PutFileAsync(FileModel file, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Putting {FileName} file into minio storage", file.Name);
        
        var putArgs = new PutObjectArgs()
            .WithObject(file.Name)
            .WithBucket(_options.Bucket)
            .WithObjectSize(file.Stream.Length)
            .WithStreamData(file.Stream)
            .WithContentType(file.ContentType);

        await _minioClient.PutObjectAsync(putArgs, cancellationToken);
        
        _logger.LogInformation("File {FileName} putted into minio storage", file.Name);

        return GetFileLink(file.Name);
    }

    public string GetFileLink(string fileName)
    {
        return $"{_options.Url}/{_options.Bucket}/{fileName}";
    }
    
    public async Task RemoveFileAsync(string fileName, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Removing {FileName} file from minio storage", fileName);
        
        var removeArgs = new RemoveObjectArgs()
            .WithBucket(_options.Bucket)
            .WithObject(fileName);
        
        await _minioClient.RemoveObjectAsync(removeArgs, cancellationToken);
        
        _logger.LogInformation("File {FileName} removed from minio storage", fileName);
    }
}