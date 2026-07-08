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
 
}
