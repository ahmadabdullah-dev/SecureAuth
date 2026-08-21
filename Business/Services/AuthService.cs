using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Business.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IEmailService _emailService;
    private readonly IUserService _userService;
    private readonly ILogger<AuthService> _logger;


    public AuthService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IEmailService emailService,
         IUserService userService,
         ILogger<AuthService> logger


    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
        _userService = userService;
        _logger = logger;
    }
    public async Task<Result<string>> RegisterAsync(RegisterDto dto)
    {
        var newUser = new AppUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            EmailConfirmed = false,
        };

        var registerResult = await _userManager.CreateAsync(newUser, dto.Password);

        if (!registerResult.Succeeded)
            return Result<string>.Failure(ServiceHelper.GetFirstError(registerResult), 400);

        var roleResult = await _userManager.AddToRoleAsync(newUser, UserRoles.MEMBER);

        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(newUser);
            return Result<string>.Failure(ServiceHelper.GetFirstError(roleResult), 400);
        }
        try
        {
            await _emailService.SendCodeAsync(newUser, "Email Confirmation", EmailPurposes.EMAIL_CONFIRMATION);
        }
        catch (Exception ex)
        {
            await _userManager.DeleteAsync(newUser);
            return Result<string>.Failure(ex.Message, 400);
        }
        return Result<string>.Success("Registered successfully.");
    }
    public async Task<Result<string>> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
            return Result<string>.Failure("Invalid email or password", 401);

        if (await _userManager.IsLockedOutAsync(user))
            return Result<string>.Failure("User is locked. Please reset the password or wait 3 Minute.", 400);

        var loginResult = await _signInManager.PasswordSignInAsync(user, dto.Password, dto.IsPersistence, true);

        if (loginResult.IsLockedOut)
            return Result<string>.Failure("User is locked. Please reset the password or wait 3 Minute.", 400);

        if (!loginResult.Succeeded)
            return Result<string>.Failure("Invalid email or password", 401);

        if (user.LockoutEnd != null)
            await _userManager.SetLockoutEndDateAsync(user, null);

        return Result<string>.Success("Logged in successfully");
    }
    public async Task<Result<string>> LogoutAsync()
    {
        await _signInManager.SignOutAsync();
        return Result<string>.Success("Logged out successfully");
    }
    public async Task<Result<string>> ForgetPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            return Result<string>.Failure("User not found", 404);
        try
        {
            await _emailService.SendCodeAsync(user, "Reset Password", EmailPurposes.PASSWORD_RESET);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result<string>.Failure("Unexpected error happened while sending Password reset code", 400);
        }

        return Result<string>.Success("Reset code sent successfully.");
    }

    public async Task<Result<string>> ResetPasswordAsync(ResetPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
            return Result<string>.Failure("Invalid or expired code.");

        var isValid = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultEmailProvider, EmailPurposes.RESET_PASSWORD, dto.Code);

        if (!isValid)
            return Result<string>.Failure("Invalid or expired code.");

        var removePasswordResult = await _userManager.RemovePasswordAsync(user);
       
        if (!removePasswordResult.Succeeded)
            return Result<string>.Failure(string.Join(",", removePasswordResult.Errors.Select(e => e.Description)));

        var addPasswordResult = await _userManager.AddPasswordAsync(user, dto.NewPassword);
        
        if (!addPasswordResult.Succeeded)
            return Result<string>.Failure(string.Join(",", addPasswordResult.Errors.Select(e => e.Description)));
       
        await _userManager.ResetAccessFailedCountAsync(user);
        await _userManager.SetLockoutEndDateAsync(user, null);

        return Result<string>.Success("Password reset successfully.");
    }
    public async Task<Result<string>> ResendEmailConfirmationCodeAsync()
    {
        var userId = _userService.GetCurrentUserId();

        if (userId == null)
            return Result<string>.Failure("Unauthorized");

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return Result<string>.Failure("Current user not found in db");

        if (await _userManager.IsEmailConfirmedAsync(user))
            return Result<string>.Failure("Email already confirmed");

        await _emailService.SendCodeAsync(user, "Email Confirmation", EmailPurposes.EMAIL_CONFIRMATION);

        return Result<string>.Success("Email Confirmation code has been sent successfully");

    }
    public async Task<Result<string>> ConfirmEmailAsync(ConfirmEmailDto dto)
    {
        var userId = _userService.GetCurrentUserId();

        if (userId == null)
            return Result<string>.Failure("Unauthorized");

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return Result<string>.Failure("Current user not found in db");

        if (await _userManager.IsEmailConfirmedAsync(user))
            return Result<string>.Failure("Email already confirmed");

        var isValid = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultEmailProvider, EmailPurposes.EMAIL_CONFIRMATION, dto.Code);

        if (!isValid)
            return Result<string>.Failure("Invalid or expired code.");

        user.EmailConfirmed = true;

        var updateResult = await _userManager.UpdateAsync(user);
        
        if(!updateResult.Succeeded)
            Result<string>.Failure("Failed to confirm email.");

        return Result<string>.Success("Email confirmed successfully.");
    }
}
