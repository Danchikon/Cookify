using System.Net.Mime;
using AutoMapper;
using Cookify.Api.Common.Controllers;
using Cookify.Application.Dtos;
using Cookify.Application.Dtos.Authentication;
using Cookify.Application.User.Authentication;
using Cookify.Application.User.Avatar;
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
    
    [HttpPost("registration")]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(
        Summary = "Registers user",
        Description = "Registers user",
        OperationId = nameof(RegisterAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Json web token has been successfully generated")]
    public async Task<IActionResult> RegisterAsync(
        [FromBody, SwaggerRequestBody(Required = true)] AuthenticateUserCommand command,
        CancellationToken cancellationToken
    )
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }
    
    [HttpGet("avatar/link")]
    [Authorize]
    [SwaggerOperation(
        Summary = "Returns avatar link for current authenticated user",
        Description = "Requires authenticated user",
        OperationId = nameof(GetAvatarLinkAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Avatar has been successfully returned", typeof(string))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User unauthorized")]
    public async Task<IActionResult> GetAvatarLinkAsync(CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetUserAvatarLinkQuery(), cancellationToken));
    }
    
    [HttpPut("avatar")]
    [Consumes("multipart/form-data")]
    [Authorize]
    [SwaggerOperation(
        Summary = "Uploads avatar for current authenticated user and returns user avatar link",
        Description = "Requires authenticated user",
        OperationId = nameof(UploadAvatarAsync)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Avatar has been successfully uploaded", typeof(string))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User unauthorized")]
    public async Task<IActionResult> UploadAvatarAsync(IFormFile formFile, CancellationToken cancellationToken)
    {
        var avatarLink = await Mediator.Send(new UploadUserAvatarCommand(
            formFile.OpenReadStream(), 
            formFile.ContentType
            ), cancellationToken);
        
        return Ok(avatarLink);
    }
    
    [HttpDelete("avatar")]
    [Authorize]
    [SwaggerOperation(
        Summary = "Deletes avatar for current authenticated user",
        Description = "Requires authenticated user",
        OperationId = nameof(DeleteAvatarAsync)
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Avatar has been successfully deleted")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User unauthorized")]
    public async Task<IActionResult> DeleteAvatarAsync(CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteUserAvatarCommand(), cancellationToken);
        return NoContent();
    }
}