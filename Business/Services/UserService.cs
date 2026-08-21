using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Business.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IHttpContextAccessor httpContextAccessor,
        UserManager<AppUser> userManager,
        IEmailService emailService,
        SignInManager<AppUser> signInManager,
        ILogger<UserService> logger

    )
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _emailService = emailService;
        _signInManager = signInManager;
        _logger = logger;
    }
    public string? GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
    public string? GetCurrentUserRole()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
    }
    public async Task<Result<UserDto>> CurrentUserAsync()
    {
        var userId = GetCurrentUserId();
        var role = GetCurrentUserRole();

        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(role))
            return Result<UserDto>.Failure("You must be logged in to perform this action.", 403);

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return Result<UserDto>.Failure("User not found!. It may have been removed or deactivated.", 404);

        var dto = new UserDto
        {
            Id = userId,
            FirstName = user.FirstName!,
            LastName = user.LastName!,
            Email = user.Email!,
            IsEmailConfirmed = user.EmailConfirmed,
            Role = role,
        };
        return Result<UserDto>.Success(dto);

    }
    public async Task<Result<string>> RequestUpdateCurrentEmailAsync(string newEmail)
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
            return Result<string>.Failure("UnAuthorized",401);

        var currentUser = await _userManager.FindByIdAsync(currentUserId);

        if (currentUser == null)
            return Result<string>.Failure("Current user not found in db",404);

        if (string.Equals(currentUser.Email, newEmail, StringComparison.OrdinalIgnoreCase))
            return Result<string>.Failure("You cannot change with the same email",409);

        var isNewEmailExists = await _userManager.FindByEmailAsync(newEmail);

        if (isNewEmailExists != null)
            return Result<string>.Failure($"Email {newEmail} already taken", 400);

        currentUser.PendingEmail = newEmail;

        var result = await _userManager.UpdateAsync(currentUser);

        try
        {
            await _emailService.SendCodeAsync(currentUser, "Email Update", EmailPurposes.EMAIL_UPDATE, newEmail);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while sending confirmation code to new email.");
            return Result<string>.Failure("Failed to send confirmation code. Please try again later.",40);
        }
        if (result.Succeeded)
            return Result<string>.Success("Confirmation code sent to new email");

        return Result<string>.Failure(ServiceHelper.GetFirstError(result),400);

    }
    public async Task<Result<string>> UpdateCurrentEmailAsync(string code)
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
            return Result<string>.Failure("UnAuthorized",401);

        var currentUser = await _userManager.FindByIdAsync(currentUserId);

        if (currentUser == null)
            return Result<string>.Failure("Current user not found in db",404);

        var isValid = await _userManager.VerifyUserTokenAsync(currentUser, TokenOptions.DefaultEmailProvider, EmailPurposes.EMAIL_UPDATE, dto.Code);

        if (!isValid)
            return Result<string>.Failure("Invalid or expired code.",400);

        if (currentUser.PendingEmail == null)
            return Result<string>.Failure("No pending email was found",404);

        currentUser.Email = currentUser.PendingEmail;

        var updateResult = await _userManager.UpdateAsync(currentUser);

        if (!updateResult.Succeeded)
            return Result<string>.Failure(string.Join(",", updateResult.Errors.Select(e => e.Description)),400);

        currentUser.PendingEmail = null;
        await _userManager.UpdateAsync(currentUser);

        return Result<string>.Success("Email updated successfully");

    }
    public async Task<Result<string>> ResendUpdateEmailConfirmationCodeAsync()
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
            return Result<string>.Failure("UnAuthorized");

        var currentUser = await _userManager.FindByIdAsync(currentUserId);

        if (currentUser == null)
            return Result<string>.Failure("current user not found in db");

        if (string.IsNullOrEmpty(currentUser.PendingEmail))
            return Result<string>.Failure("no pending email update request found. please request to update email again");

        if (currentUser.PendingEmail == null)
            return Result<string>.Failure("Please update your email then request this");

        await _userManager.UpdateAsync(currentUser);

        await _emailService.SendCodeAsync(currentUser, "Email Update", EmailPurposes.EMAIL_UPDATE, currentUser.PendingEmail);

        return Result<string>.Success("Confirmation code sent to new email");
    }
    public async Task<Result<string>> UpdateCurrentUserAsync(UpdateCurrentUserDto dto)
    {

        var currentUserId = GetCurrentUserId();
      
        if (currentUserId == null)
            return Result<string>.Failure("Unauthorized");

        var currentUser = await _userManager.FindByIdAsync(currentUserId);

        if (currentUser == null)
            return Result<string>.Failure("User not found");

        if (!string.IsNullOrWhiteSpace(dto.FirstName))
            currentUser.FirstName = dto.FirstName.Trim();

        if (!string.IsNullOrWhiteSpace(dto.LastName))
            currentUser.LastName = dto.LastName.Trim();

        if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
            currentUser.PhoneNumber = dto.PhoneNumber.Trim();

        if (!string.IsNullOrWhiteSpace(dto.Country))
            currentUser.Country = dto.Country.Trim();

        if (dto.DateOfBirth.HasValue)
            currentUser.DateOfBirth = dto.DateOfBirth.Value;

        var updateResult = await _userManager.UpdateAsync(currentUser);

        if (!updateResult.Succeeded)
            return Result<string>.Failure(string.Join(",", updateResult.Errors.Select(e => e.Description)));

        return Result<string>.Success("User updated successfully");
    }
    public async Task<Result<string>> UpdateUserNameAsync(UpdateUserNameDto dto)
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
            return Result<string>.Failure("Unauthorized");

        var currentUser = await _userManager.FindByIdAsync(currentUserId);

        if (currentUser == null)
            return Result<string>.Failure("User not found");

        if (string.Equals(currentUser.UserName, dto.NewUserName, StringComparison.OrdinalIgnoreCase))
            return Result<string>.Failure("You cannot use the same username");

        currentUser.UserName = dto.NewUserName;

        var updateResult = await _userManager.UpdateAsync(currentUser);

        if (!updateResult.Succeeded)
            return Result<string>.Failure(string.Join(",", updateResult.Errors.Select(e => e.Description)));

        return Result<string>.Success("UserName updated successfully");

    }
    public async Task<Result<string>> DeleteCurrentUserAsync()
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
            return Result<string>.Failure("Unauthorized");

        var currentUser = await _userManager.FindByIdAsync(currentUserId);

        if (currentUser == null)
            return Result<string>.Failure("User not found");

        var result = await _userManager.DeleteAsync(currentUser);

        if (result.Succeeded)
        {
            await _signInManager.SignOutAsync();
            return Result<string>.Success("User deleted successfully");

        }
        return Result<string>.Failure(string.Join(",", result.Errors.Select(e => e.Description)));

    }
}

