using Cookify.Application.Services;

namespace Cookify.Infrastructure.Services;

public class FakeTextTranslationService : ITextTranslationService
{
    public Task<string> TranslateAsync(string sourceText, string sourceLanguage, string targetLanguage, CancellationToken cancellationToken)
    {
        return Task.FromResult(sourceText + sourceLanguage + targetLanguage);
    }
}