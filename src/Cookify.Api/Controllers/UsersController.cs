using System.Net.Mime;
using AutoMapper;
using Cookify.Api.Common.Controllers;
using Cookify.Application.Common.Dtos;
using Cookify.Application.Dtos;
using Cookify.Application.Dtos.Authentication;
using Cookify.Application.Dtos.User;
using Cookify.Application.User;
using Cookify.Application.User.Authentication;
using Cookify.Application.User.Avatar;
using Cookify.Application.User.Registration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Cookify.Api.Controllers;

[Route("api/users")]
public class UsersController : ApiControllerBase
{
    public UsersController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }
    
    [HttpPost("authentication")]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(
        Summary = "Authenticates user",
        Description = "Requires already registered user",
        OperationId = nameof(AuthenticateAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Json web token has been successfully generated", typeof(JsonWebTokenDto))]
    public async Task<IActionResult> AuthenticateAsync(
        [FromBody, SwaggerRequestBody(Required = true)] AuthenticateUserCommand command,
        CancellationToken cancellationToken
    )
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }
    
    [Authorize]
    [HttpPost("current/authentication/refresh-token")]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(
        Summary = "Refreshes json web token",
        Description = "Requires authenticated user",
        OperationId = nameof(RefreshJsonWebTokenAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Json web token has been successfully refreshed", typeof(JsonWebTokenDto))]
    public async Task<IActionResult> RefreshJsonWebTokenAsync(
        [FromBody, SwaggerRequestBody(Required = true)] RefreshJsonWebTokenCommand command,
        CancellationToken cancellationToken
    )
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }
    
    [Authorize]
    [HttpDelete("current/authentication/session")]
    [SwaggerOperation(
        Summary = "Deletes current user session",
        Description = "Requires authenticated user",
        OperationId = nameof(DeleteCurrentUserSessionAsync)
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Current user session has been successfully deleted")]
    public async Task<IActionResult> DeleteCurrentUserSessionAsync(CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteCurrentUserSessionCommand(), cancellationToken);
        return NoContent();
    }
    
    [HttpPost("registration")]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(
        Summary = "Registers user",
        Description = "Registers user",
        OperationId = nameof(RegisterAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "User has been successfully registered")]
    public async Task<IActionResult> RegisterAsync(
        [FromBody, SwaggerRequestBody(Required = true)] RegisterUserCommand command,
        CancellationToken cancellationToken
    )
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }
    
    [Authorize]
    [HttpGet("current/short-info")]
    [SwaggerOperation(
        Summary = "Returns current user short info",
        Description = "Requires authenticated user",
        OperationId = nameof(GetCurrentUserShortInfoAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Current user short info has been successfully returned", typeof(UserShortInfoDto))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User unauthorized")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found", typeof(ErrorDto))]
    public async Task<IActionResult> GetCurrentUserShortInfoAsync(CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetCurrentUserShortInfoQuery(), cancellationToken));
    }
    
    [Authorize]
    [HttpGet("current")]
    [SwaggerOperation(
        Summary = "Returns current user",
        Description = "Requires authenticated user",
        OperationId = nameof(GetCurrentUserAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Current user has been successfully returned", typeof(UserDto))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User unauthorized")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found", typeof(ErrorDto))]
    public async Task<IActionResult> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetCurrentUserQuery(), cancellationToken));
    }
    
    [Authorize]
    [HttpPatch("current")]
    [SwaggerOperation(
        Summary = "Partially updates current user",
        Description = "Requires authenticated user",
        OperationId = nameof(PartiallyUpdateCurrentUserAsync)
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Current user has been successfully partially updates")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User unauthorized")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found", typeof(ErrorDto))]
    public async Task<IActionResult> PartiallyUpdateCurrentUserAsync(
        [FromBody, SwaggerRequestBody(Required = true)] PartiallyUpdateCurrentUserCommand command,
        CancellationToken cancellationToken
        )
    {
        await Mediator.Send(command, cancellationToken);
        return NoContent();
    }
    
    [Authorize]
    [HttpPost("current/avatar")]
    [Consumes("multipart/form-data")]
    [SwaggerOperation(
        Summary = "Uploads avatar for current authenticated user and returns avatar link",
        Description = "Requires authenticated user",
        OperationId = nameof(UploadCurrentUserAvatarAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Avatar has been successfully uploaded", typeof(string))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User unauthorized")]
    public async Task<IActionResult> UploadCurrentUserAvatarAsync(IFormFile formFile, CancellationToken cancellationToken)
    {
        var avatarLink = await Mediator.Send(new UploadCurrentUserAvatarCommand(
            formFile.OpenReadStream(), 
            formFile.ContentType
            ), cancellationToken);
        
        return Ok(avatarLink);
    }
    
    [Authorize]
    [HttpDelete("current/avatar")]
    [SwaggerOperation(
        Summary = "Deletes avatar for current authenticated user",
        Description = "Requires authenticated user",
        OperationId = nameof(DeleteCurrentUserAvatarAsync)
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Avatar has been successfully deleted")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User unauthorized")]
    public async Task<IActionResult> DeleteCurrentUserAvatarAsync(CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteCurrentUserAvatarCommand(), cancellationToken);
        return NoContent();
    }
}