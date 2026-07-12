using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Business.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailService;

    public UserService(
        IHttpContextAccessor httpContextAccessor,
        UserManager<AppUser> userManager,
        IEmailService emailService
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _emailService = emailService;
    }
    private const string EMAIL_UPDATE_PURPOSE = "UpdateEmail";
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
    public async Task<Result<string>> RequestUpdateEmailAsync(RequestUpdateEmailDTO dto)
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
            return Result<string>.Failure("UnAuthorized");

        var currentUser = await _userManager.FindByIdAsync(currentUserId);

        if (currentUser == null)
            return Result<string>.Failure("current user not found in db");

        if (string.Equals(currentUser.Email, dto.NewEmail, StringComparison.Ordinal))
            return Result<string>.Failure("You cannot change with the same email");

        var isNewEmailExists = await _userManager.FindByEmailAsync(dto.NewEmail);

        if (isNewEmailExists != null)
            return Result<string>.Failure($"Email {dto.NewEmail} already taken ");

        currentUser.PendingEmail = dto.NewEmail;

        await _userManager.UpdateAsync(currentUser);

        await _emailService.SendCodeAsync(currentUser, "Email Update", EMAIL_UPDATE_PURPOSE, dto.NewEmail);

        return Result<string>.Success("Confirmation code sent to new email");

    }
}

