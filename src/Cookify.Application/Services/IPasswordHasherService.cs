namespace Cookify.Application.Services;

public interface IPasswordHasherService
{
    string Hash(string password);
}