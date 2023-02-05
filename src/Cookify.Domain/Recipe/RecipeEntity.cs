using Cookify.Domain.Common.Entities;
using Cookify.Domain.Favorite;
using Cookify.Domain.Ingredient;
using Cookify.Domain.IngredientRecipe;
using Cookify.Domain.Like;
using Cookify.Domain.MealCategory;
using Cookify.Domain.User;

namespace Cookify.Domain.Recipe;

public class RecipeEntity : BaseEntity
{
    public string Title { get; set; } = null!;
    public string UkrainianTitle { get; set; } = null!;
    public string Instruction { get; set; } = null!;
    public string UkrainianInstruction { get; set; } = null!;
    public string? ImageLink { get; set; } 
    public string? PdfLink { get; set; } 
    public string? UkrainianPdfLink { get; set; } 

    public UserEntity? User { get; set; }
    public Guid CategoryId { get; set; }
    public RecipeCategoryEntity Category { get; set; } = null!;

    public ICollection<FavoriteEntity> Favorites { get; set; } = new List<FavoriteEntity>();
    public ICollection<LikeEntity> Likes { get; set; } = new List<LikeEntity>();
    public ICollection<IngredientRecipeEntity> IngredientRecipes { get; set; } = new List<IngredientRecipeEntity>();

    public RecipeEntity()
    {
        
    }
    
    public RecipeEntity(
        string title,
        string ukrainianTitle,
        string instruction,
        string ukrainianInstruction,
        Guid categoryId,
        Guid? createdBy = null
        ) : base(createdBy)
    {
        Title = title;
        UkrainianTitle = ukrainianTitle;
        Instruction = instruction;
        UkrainianInstruction = ukrainianInstruction;
        CategoryId = categoryId;
    }
}