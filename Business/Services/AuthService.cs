using DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IEmailService _emailService;

    private const string MEMBER_ROLE = "Member";

    public AuthService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IEmailService emailService

    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
    }
    public async Task<Result<string>> RegisterUserAsync(RegisterUserDto dto)   
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
      
        return Result<string>.Success("Registration successful.");
    }
    public async Task<Result<string>> LoginUserAsync(LoginUserDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
            return Result<string>.Failure("Invalid email or password");

        var loginResult = await _signInManager.PasswordSignInAsync(user, dto.Password, dto.IsPersistence, false);
        
        if (!loginResult.Succeeded)
            return Result<string>.Failure("Invalid email or password");
        
        return Result<string>.Success("Logged in successfully");
    }
    public async Task<Result<string>> LogoutUserAsync()
    {
        await _signInManager.SignOutAsync();

        return Result<string>.Success("Logged out successfully");
    }
    public async Task<Result<string>> ForgetUserPasswordAsync(ForgetUserPasswordDTO dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
            return Result<string>.Success("User not found");

        await _emailService.SendCodeAsync(user, "Reset Password", "ResetUserPassword");

        return Result<string>.Success("Reset code sent successfully.");
    }
}
