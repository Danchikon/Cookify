using Cookify.Domain.Common.Entities;
using Cookify.Domain.Ingredient.Enums;
using Cookify.Domain.Recipe;

namespace Cookify.Domain.Ingredient;

public class IngredientEntity : BaseEntity
{
    public string Title { get; set; }
    public IngredientType Type { get; set; }
    public string Description { get; set; }

    public ICollection<RecipeEntity> Recipes { get; set; } = Array.Empty<RecipeEntity>();

    public IngredientEntity(
        string title, 
        IngredientType type, 
        string description,
        Guid createdBy
    ) : base(createdBy)
    {
        Title = title;
        Type = type;
        Description = description;
    }
}