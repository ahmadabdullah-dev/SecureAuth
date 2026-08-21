using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
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
    public async Task<IActionResult> RequestUpdateCurrentEmail(string newEmail)
    {
        var result = await _userService.RequestUpdateCurrentEmailAsync(newEmail);
        return HandleResult(result);
    }
    [HttpPatch("update-current-email")]
    public async Task<IActionResult> UpdateCurrentEmail(string code)
    {
        var result = await _userService.UpdateCurrentEmailAsync(code);
        return HandleResult(result);
    }
    [HttpPost("resend-update-current-email-confirmation-code")]
    public async Task<IActionResult> ResendUpdateCurrentEmailConfirmationCode()
    {
        var result = await _userService.ResendUpdateCurrentEmailConfirmationCodeAsync();
        return HandleResult(result);
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
