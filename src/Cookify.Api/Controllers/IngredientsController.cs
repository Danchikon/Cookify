using System.Net.Mime;
using AutoMapper;
using Cookify.Api.Common.Controllers;
using Cookify.Application.Common.Dtos;
using Cookify.Application.Dtos.Ingredient;
using Cookify.Application.Ingredient;
using Cookify.Domain.Common.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Cookify.Api.Controllers;

[Route("api/ingredients")]
public class IngredientsController : ApiControllerBase
{
    public IngredientsController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }
    
    [HttpGet("{id:guid}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(
        Summary = "Returns ingredient",
        Description = "Returns ingredient",
        OperationId = nameof(GetIngredientAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Ingredient has been successfully returned", typeof(IngredientDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Ingredient not found", typeof(ErrorDto))]
    public async Task<IActionResult> GetIngredientAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetIngredientQuery(id), cancellationToken));
    }
    
    [HttpGet("{id:guid}/short-info")]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(
        Summary = "Returns ingredient short info",
        Description = "Returns ingredient short info",
        OperationId = nameof(GetIngredientShortInfoAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Ingredient short info has been successfully returned", typeof(IngredientShortInfoDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Ingredient not found", typeof(ErrorDto))]
    public async Task<IActionResult> GetIngredientShortInfoAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetIngredientShortInfoQuery(id), cancellationToken));
    }
    
    [HttpGet("short-info")]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(
        Summary = "Returns ingredient short infos paginated list",
        Description = "Returns ingredient short infos paginated list",
        OperationId = nameof(GetIngredientShortInfosAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Ingredient short infos paginated list has been successfully returned", typeof(IPaginatedList<IngredientShortInfoDto>))]
    public async Task<IActionResult> GetIngredientShortInfosAsync(
        [FromQuery] GetIngredientShortInfosQuery query, 
        CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(query, cancellationToken));
    }
    
    [HttpGet("short-info/list")]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(
        Summary = "Returns ingredient short infos list",
        Description = "Returns ingredient short infos list",
        OperationId = nameof(GetIngredientShortInfosListAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Ingredient short infos list has been successfully returned", typeof(IList<IngredientShortInfoDto>))]
    public async Task<IActionResult> GetIngredientShortInfosListAsync(
        [FromQuery] GetIngredientShortInfosListQuery query, 
        CancellationToken cancellationToken
        )
    {
        return Ok(await Mediator.Send(query, cancellationToken));
    }
}