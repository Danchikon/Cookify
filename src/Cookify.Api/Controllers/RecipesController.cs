using System.Net.Mime;
using AutoMapper;
using Cookify.Api.Common.Controllers;
using Cookify.Application.Common.Dtos;
using Cookify.Application.Dtos.Recipe;
using Cookify.Application.Dtos.RecipeCategory;
using Cookify.Application.Recipe;
using Cookify.Application.Recipe.Favorite;
using Cookify.Application.Recipe.Ingredient;
using Cookify.Application.Recipe.Like;
using Cookify.Application.RecipeCategory;
using Cookify.Domain.Common.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Cookify.Api.Controllers;

[Route("api/recipes")]
public class RecipesController : ApiControllerBase
{
    public RecipesController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }
    
    [HttpGet("{id:guid}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(
        Summary = "Returns recipe",
        Description = "Returns recipe",
        OperationId = nameof(GetRecipeAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Recipe has been successfully returned", typeof(RecipeDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Recipe not found", typeof(ErrorDto))]
    public async Task<IActionResult> GetRecipeAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetRecipeQuery(id), cancellationToken));
    }
    
    [Authorize]
    [HttpPost]
    [Consumes("multipart/form-data")]
    [SwaggerOperation(
        Summary = "Creates recipe",
        Description = "Requires authenticated user",
        OperationId = nameof(CreateRecipeAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Recipe has been successfully created", typeof(Guid))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User unauthorized", typeof(ErrorDto))]
    public async Task<IActionResult> CreateRecipeAsync(
        IFormFile image,
        [FromForm] CreateRecipeDto dto,
        CancellationToken cancellationToken
        )
    {
        var command = new CreateRecipeCommand
        {
            UkrainianTitle = dto.UkrainianTitle,
            UkrainianInstruction = dto.UkrainianInstruction,
            CategoryId = dto.CategoryId,
            IsPublic = dto.IsPublic,
            ImageStream = image.OpenReadStream(),
            ImageContentType = image.ContentType
        };
        
        return Ok(await Mediator.Send(command, cancellationToken));
    }
    
    [Authorize]
    [HttpPost("{recipeId:guid}/ingredients/{ingredientId:guid}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(
        Summary = "Adds ingredient to recipe",
        Description = "Requires authenticated user",
        OperationId = nameof(AddIngredientToRecipeAsync)
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Ingredients has been successfully added to recipe", typeof(Guid))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User unauthorized", typeof(ErrorDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Recipe or ingredient not found", typeof(ErrorDto))]
    public async Task<IActionResult> AddIngredientToRecipeAsync(
        Guid recipeId,
        Guid ingredientId,
        [FromBody] string ukrainianMeasure,
        CancellationToken cancellationToken
    )
    {
        await Mediator.Send(new AddIngredientToRecipeCommand(ingredientId, recipeId, ukrainianMeasure), cancellationToken);
        return NoContent();
    }
    
    [Authorize]
    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
        Summary = "Deletes recipe",
        Description = "Requires authenticated user",
        OperationId = nameof(DeleteRecipeAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Recipe has been successfully deleted")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User unauthorized", typeof(ErrorDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Recipe not found", typeof(ErrorDto))]
    public async Task<IActionResult> DeleteRecipeAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteRecipeCommand(id), cancellationToken);
        return NoContent();
    }
    
    [HttpPost("{id:guid}/users/current/favorite")]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(
        Summary = "Creates new favorite recipe for current authenticated user",
        Description = "Requires authenticated user",
        OperationId = nameof(CreateFavoriteRecipeAsync)
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Favorite recipe has been successfully created")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Recipe not found", typeof(ErrorDto))]
    public async Task<IActionResult> CreateFavoriteRecipeAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new CreateFavoriteRecipeCommand(id), cancellationToken);
        return NoContent();
    }
    
    [HttpDelete("{id:guid}/users/current/favorite")]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(
        Summary = "Deletes favorite recipe for current authenticated user",
        Description = "Requires authenticated user",
        OperationId = nameof(DeleteFavoriteRecipeAsync)
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Favorite recipe has been successfully deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Recipe not found", typeof(ErrorDto))]
    public async Task<IActionResult> DeleteFavoriteRecipeAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteFavoriteRecipeCommand(id), cancellationToken);
        return NoContent();
    }
    
    [HttpPost("{id:guid}/users/current/like")]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(
        Summary = "Creates new like recipe for current authenticated user",
        Description = "Requires authenticated user",
        OperationId = nameof(CreateLikeRecipeAsync)
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Like recipe has been successfully created")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Recipe not found", typeof(ErrorDto))]
    public async Task<IActionResult> CreateLikeRecipeAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new CreateLikeRecipeCommand(id), cancellationToken);
        return NoContent();
    }
    
    [HttpDelete("{id:guid}/users/current/like")]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(
        Summary = "Deletes like recipe for current authenticated user",
        Description = "Requires authenticated user",
        OperationId = nameof(DeleteLikeRecipeAsync)
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Like recipe has been successfully deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Recipe not found", typeof(ErrorDto))]
    public async Task<IActionResult> DeleteLikeRecipeAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteLikeRecipeCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpGet("short-info")]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(
        Summary = "Returns recipe short infos paginated list",
        Description = "Returns recipe short infos paginated list",
        OperationId = nameof(GetRecipeShortInfosAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Recipe short infos paginated list has been successfully returned", typeof(IPaginatedList<RecipeCategoryShortInfoDto>))]
    public async Task<IActionResult> GetRecipeShortInfosAsync(
        [FromQuery] GetRecipeShortInfosQuery query, 
        CancellationToken cancellationToken
        )
    {
        return Ok(await Mediator.Send(query, cancellationToken));
    }
    
    [HttpGet("short-info/random")]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(
        Summary = "Returns random recipe short infos paginated list",
        Description = "Returns random recipe short infos paginated list",
        OperationId = nameof(GetRandomRecipeShortInfosAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Random recipe short infos paginated list has been successfully returned", typeof(IPaginatedList<RecipeCategoryShortInfoDto>))]
    public async Task<IActionResult> GetRandomRecipeShortInfosAsync(
        [FromQuery] GetRandomRecipeShortInfosQuery query, 
        CancellationToken cancellationToken
        )
    {
        return Ok(await Mediator.Send(query, cancellationToken));
    }
}