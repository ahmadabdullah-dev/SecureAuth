using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/user")]
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
    public async Task<IActionResult> RequestUpdateEmail(RequestUpdateEmailDTO dto)
    {
        var result = await _userService.RequestUpdateEmailAsync(dto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);

    }
    [HttpPatch("update-email")]
    public async Task<IActionResult> UpdateEmail(UpdateEmailDTO dto)
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
    public async Task<IActionResult> UpdateCurrentUser(UpdateCurrentUserDTO dto)
    {
        var result = await _userService.UpdateCurrentUserAsync(dto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
