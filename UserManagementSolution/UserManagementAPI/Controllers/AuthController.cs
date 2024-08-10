using Asp.Versioning;
using Mapster;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models.DTOs;
using UserManagement.Services.Contracts;

namespace UserManagementAPI.Controllers;
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;
    private readonly IPublishEndpoint _publish;

    public AuthController(IAuthService authService, IUserService userService, IPublishEndpoint publish)
    {
        _authService = authService;
        _userService = userService;
        _publish = publish;
    }

    [HttpPost("login"), MapToApiVersion( 1.0 )]
    public async Task<IActionResult> Login([FromBody] LoginModel user)
    {
        var token = await _authService.Authenticate(user);
        if (token == null)
        {
            return Unauthorized();
        }

        return Ok(new { Token = token });
    }

    [HttpGet("GetUser")]
    [Authorize]
    public async Task<IActionResult> GetUser()
    {
        var currentUser = await _authService.GetCurrentUser();
        var User =await _userService.GetUserData(currentUser.UserName);
        if (User == null)
        {
            return NotFound();
        }

        return Ok(new { User });
    }

    [HttpPost("AddUser")]
    public async Task<IActionResult> AddUser([FromBody] NewUserCommandModel newUserCommandModel)
    {
        var newUserId = await _userService.AddUser(newUserCommandModel.Adapt<AddNewUserModel>());
        if (newUserId > 0)
        {
            await _publish.Publish(newUserCommandModel);
        }
        else
        {
            throw new Exception("Can Not Insert User");
        }

        return Ok(new { newUserId });
    }
}