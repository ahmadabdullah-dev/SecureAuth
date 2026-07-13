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
    [HttpPatch("assign-to-member-role")]
    public async Task<IActionResult> AssignToMemberRole(string username)
    {
        var result = await _adminService.AssignToMemberRole(username);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpPatch("assign-to-admin-role")]
    public async Task<IActionResult> AssignToAdminRole(string username)
    {
        var result = await _adminService.AssignToAdminRole(username);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
