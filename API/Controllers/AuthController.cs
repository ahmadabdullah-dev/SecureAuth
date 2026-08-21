using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers;

[ApiController]
[Route("api/auth")]
[EnableRateLimiting("rateLimiter")]

public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;   
    }   
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var result = await _authService.RegisterAsync(dto);
        return HandleResult(result);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        return HandleResult(result);
    }
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var result = await _authService.LogoutAsync();

        return result.IsSuccess ? Ok(result) : Unauthorized(result);
    }
    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword(ForgetPasswordDto dto)
    {
        var result = await _authService.ForgetPasswordAsync(dto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
    {
        var result = await _authService.ResetPasswordAsync(dto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [Authorize]
    [HttpPost("resend-email-confirmation-code")]
    public async Task<IActionResult> ResendEmailConfirmationCode()
    {
        var result = await _authService.ResendEmailConfirmationCodeAsync();
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [Authorize]
    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto dto)
    {
        var result = await _authService.ConfirmEmailAsync(dto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
