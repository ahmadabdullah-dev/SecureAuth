using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/auth")]

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;   
    }   
    
    [HttpPost("register-user")]
    public async Task<IActionResult> RegisterUser(RegisterUserDto dto)
    {
        var result = await _authService.RegisterUserAsync(dto);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpPost("login-user")]
    public async Task<IActionResult> LoginUser(LoginUserDto dto)
    {
        var result = await _authService.LoginUserAsync(dto);

        return result.IsSuccess ? Ok(result) : Unauthorized(result);
    }
    [Authorize]
    [HttpPost("logout-user")]
    public async Task<IActionResult> LogoutUser()
    {
        var result = await _authService.LogoutUserAsync();

        return result.IsSuccess ? Ok(result) : Unauthorized(result);
    }
    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetUserPassword(ForgetUserPasswordDTO dto)
    {
        var result = await _authService.ForgetUserPasswordAsync(dto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetUserPassword(ResetUserPasswordDTO dto)
    {
        var result = await _authService.ResetUserPasswordAsync(dto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
