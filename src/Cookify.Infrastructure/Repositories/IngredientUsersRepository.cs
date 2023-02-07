using Cookify.Domain.Exceptions;
using Cookify.Domain.IngredientUser;
using Cookify.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cookify.Infrastructure.Repositories;

public class IngredientUsersRepository : IIngredientUsersRepository
{
    private readonly CookifyDbContext _dbContext;
    
    public IngredientUsersRepository(CookifyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddAsync(IngredientUserEntity ingredientUser, CancellationToken cancellationToken)
    {
        await _dbContext.IngredientUsers.AddAsync(ingredientUser, cancellationToken);
    }

    public ValueTask UpdateAsync(IngredientUserEntity ingredientUser, CancellationToken cancellationToken)
    {
        _dbContext.IngredientUsers.Update(ingredientUser);
        
        return ValueTask.CompletedTask;
    }

    public async Task<IngredientUserEntity?> FirstOrDefaultAsync(Guid userId, Guid ingredientId, CancellationToken cancellationToken)
    {
        return await _dbContext.IngredientUsers.FirstOrDefaultAsync(
            ingredientUser => ingredientUser.IngredientId == ingredientId && ingredientUser.UserId == userId, 
            cancellationToken
            );
    }
    

    public ValueTask RemoveAsync(IngredientUserEntity ingredientUser, CancellationToken cancellationToken)
    {
         _dbContext.IngredientUsers.Remove(ingredientUser);

         return ValueTask.CompletedTask;
    }

    public async Task<IngredientUserEntity> FirstAsync(Guid userId, Guid ingredientId, CancellationToken cancellationToken)
    {
        var entity = await FirstOrDefaultAsync(userId, ingredientId, cancellationToken);

        if (entity is null)
        {
            throw NotFoundException.Create<IngredientUserEntity>();
        }

        return entity;
    }
}