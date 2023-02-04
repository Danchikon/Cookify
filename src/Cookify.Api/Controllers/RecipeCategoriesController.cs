using System.Net.Mime;
using AutoMapper;
using Cookify.Api.Common.Controllers;
using Cookify.Application.Common.Dtos;
using Cookify.Application.Dtos.RecipeCategory;
using Cookify.Application.MealCategory;
using Cookify.Application.RecipeCategory;
using Cookify.Domain.Common.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Cookify.Api.Controllers;

[Route("api/recipe-categories")]
public class RecipeCategoriesController : ApiControllerBase
{
    public RecipeCategoriesController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }
    
    [HttpGet("{id:guid}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(
        Summary = "Returns recipe category",
        Description = "Returns recipe category",
        OperationId = nameof(GetRecipeCategoryAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Recipe category has been successfully returned", typeof(RecipeCategoryDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Recipe category not found", typeof(ErrorDto))]
    public async Task<IActionResult> GetRecipeCategoryAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetRecipeCategoryQuery(id), cancellationToken));
    }
    
    [HttpGet("{id:guid}/short-info")]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(
        Summary = "Returns recipe category short info",
        Description = "Returns recipe category short info",
        OperationId = nameof(GetRecipeCategoryShortInfoAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Recipe category short info has been successfully returned", typeof(RecipeCategoryShortInfoDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Recipe category not found", typeof(ErrorDto))]
    public async Task<IActionResult> GetRecipeCategoryShortInfoAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetRecipeCategoryShortInfoQuery(id), cancellationToken));
    }
    
    [HttpGet("short-info")]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(
        Summary = "Returns recipe category short infos paginated list",
        Description = "Returns recipe category short infos paginated list",
        OperationId = nameof(GetRecipeCategoryShortInfosAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Recipe category short infos paginated list has been successfully returned", typeof(IPaginatedList<RecipeCategoryShortInfoDto>))]
    public async Task<IActionResult> GetRecipeCategoryShortInfosAsync(
        [FromQuery] GetRecipeCategoryShortInfosQuery query, 
        CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(query, cancellationToken));
    }
    
    [HttpGet("short-info/list")]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(
        Summary = "Returns recipe category short infos list",
        Description = "Returns recipe category short infos list",
        OperationId = nameof(GetRecipeCategoryShortInfosListAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Recipe category short infos list has been successfully returned", typeof(IList<RecipeCategoryShortInfoDto>))]
    public async Task<IActionResult> GetRecipeCategoryShortInfosListAsync(
        [FromQuery] GetRecipeCategoryShortInfosListQuery query, 
        CancellationToken cancellationToken
        )
    {
        return Ok(await Mediator.Send(query, cancellationToken));
    }
}