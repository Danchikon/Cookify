using Cookify.Domain.Common.Repositories;

namespace Cookify.Domain.IngredientUser;

public interface IIngredientUsersRepository
{
    Task AddAsync(IngredientUserEntity ingredientUser, CancellationToken cancellationToken);
    ValueTask UpdateAsync(IngredientUserEntity ingredientUser, CancellationToken cancellationToken);
    Task<IngredientUserEntity?> FirstOrDefaultAsync(Guid userId, Guid ingredientId, CancellationToken cancellationToken);
    Task<IngredientUserEntity> FirstAsync(Guid userId, Guid ingredientId, CancellationToken cancellationToken);
    ValueTask RemoveAsync(IngredientUserEntity ingredientUser, CancellationToken cancellationToken);
}