using Cookify.Domain.Common.Entities;
using Cookify.Domain.Ingredient;
using Cookify.Domain.User;

namespace Cookify.Domain.IngredientUser;

public class IngredientUserEntity 
{
    public Guid IngredientId { get; init; }
    public IngredientEntity Ingredient { get; set; } = null!;
    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;
    public string UkrainianMeasure { get; set; } = null!;

    public IngredientUserEntity()
    {
        
    }
    
    public IngredientUserEntity(
        Guid ingredientId,
        Guid userId,
        string ukrainianMeasure
        )
    {
        UserId = userId;
        IngredientId = ingredientId;
        UkrainianMeasure = ukrainianMeasure;
    }
}