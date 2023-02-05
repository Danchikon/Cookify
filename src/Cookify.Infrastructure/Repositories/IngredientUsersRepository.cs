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
    
    public async Task AddAsync(IngredientUserEntity ingredientUser)
    {
        await _dbContext.IngredientUsers.AddAsync(ingredientUser);
    }

    public ValueTask UpdateAsync(IngredientUserEntity ingredientUser)
    {
        _dbContext.IngredientUsers.Update(ingredientUser);
        
        return ValueTask.CompletedTask;
    }

    public async Task<IngredientUserEntity?> FirstOrDefaultAsync(Guid userId, Guid ingredientId)
    {
        return await _dbContext.IngredientUsers.FirstOrDefaultAsync(ingredientUser => ingredientUser.IngredientId == ingredientId && ingredientUser.UserId == userId);
    }
    

    public ValueTask RemoveAsync(IngredientUserEntity ingredientUser)
    {
         _dbContext.Remove(ingredientUser);

         return ValueTask.CompletedTask;
    }

    public async Task<IngredientUserEntity> FirstAsync(Guid userId, Guid ingredientId)
    {
        var entity = await FirstOrDefaultAsync(userId, ingredientId);

        if (entity is null)
        {
            throw NotFoundException.Create<IngredientUserEntity>();
        }

        return entity;
    }
}