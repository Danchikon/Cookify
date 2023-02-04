using Cookify.Domain.Common.Entities;
using Cookify.Domain.Recipe;

namespace Cookify.Domain.MealCategory;

public class RecipeCategoryEntity : BaseEntity
{
    public string Name { get; set; } = null!;
    public string UkrainianName { get; set; } = null!;
    public string? Description { get; set; }
    public string? UkrainianDescription { get; set; }
    public string? ImageLink { get; set; }

    public ICollection<RecipeEntity> Recipes { get; set; } = new List<RecipeEntity>();

    public RecipeCategoryEntity()
    {
        
    }
    
    public RecipeCategoryEntity(
        string name, 
        string ukrainianName, 
        string? imageLink = null,
        string? description = null,
        string? ukrainianDescription = null
        )
    {
        Name = name;
        UkrainianName = ukrainianName;
        Description = description;
        UkrainianDescription = ukrainianDescription;
        ImageLink = imageLink;
    }
}