using Cookify.Domain.Common.Repositories;

namespace Cookify.Domain.IngredientUser;

public interface IIngredientUsersRepository
{
    Task AddAsync(IngredientUserEntity ingredientUser);
    ValueTask UpdateAsync(IngredientUserEntity ingredientUser);
    Task<IngredientUserEntity?> FirstOrDefaultAsync(Guid userId, Guid ingredientId);
    Task<IngredientUserEntity> FirstAsync(Guid userId, Guid ingredientId);
    ValueTask RemoveAsync(IngredientUserEntity ingredientUser);
}