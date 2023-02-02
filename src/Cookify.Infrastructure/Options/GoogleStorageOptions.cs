using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Cookify.Infrastructure.Options;

public sealed class GoogleStorageOptions
{
    public const string SectionName = "GoogleStorage";
    
    public string? Url { get; init; }
    
    public string? ProjectId { get; init; }
    public string? CredentialFileJson { get; init; } 
    public string? Bucket { get; init; }
}