using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/user")]
[EnableRateLimiting("rateLimiter")]

public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpGet("current-user")]
    public async Task<IActionResult> CurrentUser()
    {
        var result = await _userService.CurrentUserAsync();
        return result.IsSuccess ? Ok(result) : Unauthorized(result);
    }
    
    [HttpPost("request-update-email")]
    public async Task<IActionResult> RequestUpdateEmail(RequestUpdateEmailDto dto)
    {
        var result = await _userService.RequestUpdateEmailAsync(dto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);

    }
    [HttpPatch("update-email")]
    public async Task<IActionResult> UpdateEmail(UpdateEmailDto dto)
    {
        var result = await _userService.UpdateEmailAsync(dto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);

    }
    [HttpPost("resend-update-email-confirmation-code")]
    public async Task<IActionResult> ResendUpdateEmailConfirmationCode()
    {
        var result = await _userService.ResendUpdateEmailConfirmationCodeAsync();
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpPut("update-current-user")]
    public async Task<IActionResult> UpdateCurrentUser(UpdateCurrentUserDto dto)
    {
        var result = await _userService.UpdateCurrentUserAsync(dto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpPatch("update-current-username")]
    public async Task<IActionResult> UpdatUserName(UpdateUserNameDto dto)
    {
        var result = await _userService.UpdateUserNameAsync(dto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpDelete("delete-current-user")]
    public async Task<IActionResult> DeleteCurrentUser()
    {
        var result = await _userService.DeleteCurrentUserAsync();
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpGet("current-user-role")]
    public async Task<IActionResult> GetCurrentUserRole()
    {
        var result =  _userService.GetCurrentUserRole();
        return result == null ? Ok(result) : BadRequest(result);
    }
}
