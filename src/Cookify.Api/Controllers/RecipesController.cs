using System.Net.Mime;
using AutoMapper;
using Cookify.Api.Common.Controllers;
using Cookify.Application.Common.Dtos;
using Cookify.Application.Dtos.Recipe;
using Cookify.Application.Dtos.RecipeCategory;
using Cookify.Application.MealCategory;
using Cookify.Application.Recipe;
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
}