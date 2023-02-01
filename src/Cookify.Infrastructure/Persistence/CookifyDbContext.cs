using System.Reflection;
using Cookify.Domain.Favorite;
using Cookify.Domain.Ingredient;
using Cookify.Domain.IngredientRecipe;
using Cookify.Domain.Like;
using Cookify.Domain.Recipe;
using Cookify.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace Cookify.Infrastructure.Persistence;

public class CookifyDbContext : DbContext
{

    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<RecipeEntity> Recipes { get; set; } = null!;
    public DbSet<IngredientEntity> Ingredients { get; set; } = null!;
    public DbSet<IngredientRecipeEntity> IngredientRecipes { get; set; } = null!;
    public DbSet<FavoriteEntity> Favorites { get; set; } = null!;
    public DbSet<LikeEntity> Likes { get; set; } = null!;

    public CookifyDbContext(DbContextOptions<CookifyDbContext> dbContextOptions) : base(dbContextOptions)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}