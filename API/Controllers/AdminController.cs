using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Policy = "AdminOrSuperAdmin")]
[Route("api/admin")]
[ApiController]
public class AdminController : ControllerBase
{
    IAdminService _adminService;
    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers([FromQuery] PaginationParams p)
    {
        var result = await _adminService.GetUsersAsync(p);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

}
