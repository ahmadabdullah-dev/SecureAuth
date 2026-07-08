

using DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
   
    private const string MEMBER_ROLE = "Member";

    public AuthService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
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
                return Result<string>.Failure(string.Join(",", registerResult.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(newUser, MEMBER_ROLE);
      
            return Result<string>.Success("Registration successful.");

        }
    }
