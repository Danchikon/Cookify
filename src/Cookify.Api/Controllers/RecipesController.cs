using System.Net.Mime;
using AutoMapper;
using Cookify.Api.Common.Controllers;
using Cookify.Application.Common.Dtos;
using Cookify.Application.Dtos.Recipe;
using Cookify.Application.Dtos.RecipeCategory;
using Cookify.Application.Recipe;
using Cookify.Application.Recipe.Favorite;
using Cookify.Application.Recipe.Like;
using Cookify.Application.RecipeCategory;
using Cookify.Domain.Common.Pagination;
using MediatR;
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
        CancellationToken cancellationToken)
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
        CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(query, cancellationToken));
    }
}