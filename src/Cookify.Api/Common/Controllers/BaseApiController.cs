using AutoMapper;
using Cookify.Application.Common.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Cookify.Api.Common.Controllers;

[ApiController]
[SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request", typeof(ErrorDto))]
[SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ErrorDto))]
public abstract class ApiControllerBase : ControllerBase
{
    protected readonly IMediator Mediator;
    protected readonly IMapper Mapper;
    
    protected ApiControllerBase(IMediator mediator, IMapper mapper)
    {
        Mediator = mediator;
        Mapper = mapper;
    }
}