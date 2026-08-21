using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers;

[Authorize]
[EnableRateLimiting("rateLimiter")]

public class UserController : BaseApiController
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpGet("current")]
    public async Task<IActionResult> CurrentUser()
    {
        var result = await _userService.CurrentUserAsync();
        return HandleResult(result);
    }
    
    [HttpPost("request-update-current-email")]
    public async Task<IActionResult> RequestUpdateCurrentEmail(RequestUpdateEmailDto dto)
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
