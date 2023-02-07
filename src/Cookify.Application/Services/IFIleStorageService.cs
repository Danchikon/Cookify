using Cookify.Application.Models;

namespace Cookify.Application.Services;

public interface IFileStorageService
{
    Task<string> PutFileAsync(FileModel file, CancellationToken cancellationToken);
    string GetFileLink(string fileName);
    Task RemoveFileAsync(string fileName, CancellationToken cancellationToken);
}