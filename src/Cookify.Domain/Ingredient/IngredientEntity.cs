using Cookify.Domain.Common.Entities;
using Cookify.Domain.IngredientRecipe;
using Cookify.Domain.IngredientUser;

namespace Cookify.Domain.Ingredient;

public class IngredientEntity : BaseEntity
{
    public string Name { get; set; } = null!;
    public string UkrainianName { get; set; } = null!;
    public string? Description { get; set; }
    public string? UkrainianDescription { get; set; }
    public string? ImageLink { get; set; }

    public ICollection<IngredientRecipeEntity> IngredientRecipes { get; set; } = new List<IngredientRecipeEntity>();
    public ICollection<IngredientUserEntity> IngredientUsers { get; set; } = new List<IngredientUserEntity>();

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