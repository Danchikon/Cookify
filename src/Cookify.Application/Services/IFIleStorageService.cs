using Cookify.Application.Models;

namespace Cookify.Application.Services;

public interface IFileStorageService
{
    Task<string> PutFileAsync(FileModel file, CancellationToken cancellationToken);
    string GetFileLink(string fileName);
    Task<FileModel> GetFileAsync(string fileName, CancellationToken cancellationToken);
    Task RemoveFileAsync(string fileName, CancellationToken cancellationToken);
}