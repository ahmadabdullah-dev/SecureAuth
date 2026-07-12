using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IEmailService _emailService;
    private readonly IUserService _userService;

    private const string MEMBER_ROLE = "Member";

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

        await _userManager.AddToRoleAsync(newUser, MEMBER_ROLE);
        
        await _emailService.SendCodeAsync(newUser, "Email Confirmation", "EmailConfirmation");

        return Result<string>.Success("Registration successful. A confirmation code has been sent to your email.");
    }
    public async Task<Result<string>> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
            return Result<string>.Failure("Invalid email or password");

        var loginResult = await _signInManager.PasswordSignInAsync(user, dto.Password, dto.IsPersistence, false);
        
        if (!loginResult.Succeeded)
            return Result<string>.Failure("Invalid email or password");
        
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

        await _emailService.SendCodeAsync(user, "Reset Password", "ResetPassword");

        return Result<string>.Success("Reset code sent successfully.");
    }
    public async Task<Result<string>> ResetPasswordAsync(ResetPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
            return Result<string>.Failure("Invalid or expired code.");

        var isValid = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultEmailProvider, "ResetPassword", dto.Code);

        if (!isValid)
            return Result<string>.Failure("Invalid or expired code.");

        var removePasswordResult = await _userManager.RemovePasswordAsync(user);
       
        if (!removePasswordResult.Succeeded)
            return Result<string>.Failure(string.Join(",", removePasswordResult.Errors.Select(e => e.Description)));

        var addPasswordResult = await _userManager.AddPasswordAsync(user, dto.NewPassword);
        
        if (!addPasswordResult.Succeeded)
            return Result<string>.Failure(string.Join(",", addPasswordResult.Errors.Select(e => e.Description)));

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

        await _emailService.SendCodeAsync(user, "Email Confirmation", "EmailConfirmation");

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

        var isValid = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultEmailProvider, "EmailConfirmation", dto.Code);

        if (!isValid)
            return Result<string>.Failure("Invalid or expired code.");

        user.EmailConfirmed = true;

        var updateResult = await _userManager.UpdateAsync(user);
        
        if(!updateResult.Succeeded)
            Result<string>.Failure("Failed to confirm email.");

        return Result<string>.Success("Email confirmed successfully.");
    }
}
