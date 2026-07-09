using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Business.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<AppUser> _userManager;

    public UserService(
        IHttpContextAccessor httpContextAccessor,
        UserManager<AppUser> userManager
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }
    public string? GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
    public async Task<Result<UserDto>> CurrentUserAsync()
    {
        var userId = GetCurrentUserId();

        if (string.IsNullOrEmpty(userId))
            return Result<UserDto>.Failure("You must be logged in to perform this action.");

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return Result<UserDto>.Failure("We couldn't find your account. It may have been removed or deactivated.");

        var roles = await _userManager.GetRolesAsync(user);

        var userDTO = new UserDto
        (
             user.UserName!,
             user.FirstName,
             user.LastName,
             user.Email!,
             user.PhoneNumber,
             user.Country,
             user.EmailConfirmed,
             user.DateOfBirth,
             user.CreatedDate,
             roles
        );

        return Result<UserDto>.Success(userDTO);
    }
}

