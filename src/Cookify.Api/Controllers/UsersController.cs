using AutoMapper;
using Cookify.Api.Common.Controllers;
using Cookify.Application.User;
using Cookify.Application.User.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Cookify.Api.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ApiControllerBase
{
    public UsersController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }
    
    [HttpPost("authentication")]
    [SwaggerOperation(
        Summary = "Authenticates user",
        Description = "Requires already registered user",
        OperationId = nameof(Authenticate)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Json web token has been successfully generated")]
    public async Task<IActionResult> Authenticate(
        [FromBody, SwaggerRequestBody(Required = true)] AuthenticateUserCommand command,
        CancellationToken cancellationToken
    )
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }
    
    [HttpPost("registration")]
    [SwaggerOperation(
        Summary = "Registers user",
        Description = "Registers user",
        OperationId = nameof(Register)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Json web token has been successfully generated")]
    public async Task<IActionResult> Register(
        [FromBody, SwaggerRequestBody(Required = true)] AuthenticateUserCommand command,
        CancellationToken cancellationToken
    )
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }
    
}