namespace Cookify.Application.Services;

public interface ITextTranslationService
{
    Task<string> TranslateAsync(string sourceText, string sourceLanguage, string targetLanguage);
}