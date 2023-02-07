using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Cookify.Infrastructure.Options;

public sealed record GoogleStorageOptions
{
    public const string SectionName = "GoogleStorage";

    public string Url { get; init; } = null!;
    public string CredentialFileJson { get; init; } = null!;
    public string Bucket { get; init; } = null!;
    public int MaximumConcurrency { get; init; }
}