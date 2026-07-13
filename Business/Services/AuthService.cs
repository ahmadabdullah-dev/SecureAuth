using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IEmailService _emailService;
    private readonly IUserService _userService;


    public AuthService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IEmailService emailService,
         IUserService userService


    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
        _userService = userService;
    }
    public async Task<Result<string>> RegisterAsync(RegisterDto dto)   
    {
        var newUser = new AppUser
        {
                UserName = dto.UserName,
                Email = dto.Email,
                EmailConfirmed = false
        };

        var registerResult = await _userManager.CreateAsync(newUser,dto.Password);
        
        if (!registerResult.Succeeded)
            return Result<string>.Failure(registerResult.Errors.FirstOrDefault()?.Description ?? "Unexpected error happened");

        await _userManager.AddToRoleAsync(newUser, UserRoles.MEMBER);
        
        await _emailService.SendCodeAsync(newUser, "Email Confirmation", EmailPurposes.EMAIL_CONFIRMATION);

        return Result<string>.Success("Registration successful. A confirmation code has been sent to your email. Please make sure to login and confirm your email in 15 minute otherwise you will be deleted");
    }
    public async Task<Result<string>> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
            return Result<string>.Failure("Invalid email or password");
      
        if (await _userManager.IsLockedOutAsync(user))
            return Result<string>.Failure("User is locked. Please reset the password or wait 3 Minute.");

        var loginResult = await _signInManager.PasswordSignInAsync(user, dto.Password, dto.IsPersistence, false);
        
        if (!loginResult.Succeeded)
        {
            await _userManager.AccessFailedAsync(user);
            return Result<string>.Failure("Invalid email or password");

        }
        await _userManager.ResetAccessFailedCountAsync(user);
        await _userManager.SetLockoutEndDateAsync(user, null);

        return Result<string>.Success("Logged in successfully");
    }
    public async Task<Result<string>> LogoutAsync()
    {
        await _signInManager.SignOutAsync();

        return Result<string>.Success("Logged out successfully");
    }
    public async Task<Result<string>> ForgetPasswordAsync(ForgetPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
            return Result<string>.Failure("User not found");

        await _emailService.SendCodeAsync(user, "Reset Password", EmailPurposes.RESET_PASSWORD);

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
