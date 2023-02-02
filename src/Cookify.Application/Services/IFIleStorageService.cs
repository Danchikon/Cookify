using Cookify.Application.Models;

namespace Cookify.Application.Services;

public interface IFileStorageService
{
    Task<string> PutFileAsync(FileModel file);
    string GetFileLink(string fileName);
    Task<FileModel> GetFileAsync(string fileName);
    Task RemoveFileAsync(string fileName);
}