namespace Cookify.Infrastructure.Options;

public class MinioStorageOptions
{
    public const string SectionName = "MinioStorage";

    public string Login { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string Endpoint { get; init; } = null!;
    public int Port { get; init; }
    public string Bucket { get; init; } = null!;
    public string Url { get; init; } = null!;
}