using Cookify.Domain.Common.Entities;
using Cookify.Domain.IngredientRecipe;

namespace Cookify.Domain.Ingredient;

public class IngredientEntity : BaseEntity
{
    public string Name { get; set; } = null!;
    public string UkrainianName { get; set; } = null!;
    public string? Description { get; set; }
    public string? UkrainianDescription { get; set; }
    public string? ImageLink { get; set; } 

    public ICollection<IngredientRecipeEntity> IngredientRecipes { get; set; } = Array.Empty<IngredientRecipeEntity>();

    public IngredientEntity() 
    {
        
    }
    
    public IngredientEntity(
        string name, 
        string ukrainianName,
        string? imageLink = null,
        string? description = null,
        string? ukrainianDescription = null,
        Guid? createdBy = null
    ) : base(createdBy)
    {
        Name = name;
        UkrainianName = ukrainianName;
        ImageLink = imageLink;
        Description = description;
        UkrainianDescription = ukrainianDescription;
    }
}