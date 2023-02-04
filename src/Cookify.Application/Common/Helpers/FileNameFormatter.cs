namespace Cookify.Application.Common.Helpers;

public static class FileNameFormatter
{
    public static string FormatForUserAvatar(Guid userId)
    {
        return $"user-avatars/{GuidToShortStringConverter.FromGuidToShortString(userId)}";
    }
    
    public static string FormatForRecipeCategoryImage(Guid categoryId)
    {
        return $"recipe-categories/{GuidToShortStringConverter.FromGuidToShortString(categoryId)}";
    }
    
    public static string FormatForIngredientImage(Guid ingredientId)
    {
        return $"ingredients/{GuidToShortStringConverter.FromGuidToShortString(ingredientId)}";
    }
    
    public static string FormatForRecipeImage(Guid ingredientId)
    {
        return $"recipes/{GuidToShortStringConverter.FromGuidToShortString(ingredientId)}";
    }
}