using DataAccess;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class AdminService : IAdminService
{
    private readonly ApplicationDbContext _appDbContext;
    private readonly IUserService _userService;
    private readonly UserManager<AppUser> _userManager;

  

    public AdminService(
        ApplicationDbContext appDbContext, 
        IUserService userService,
        UserManager<AppUser> userManager)
    {
        _appDbContext = appDbContext;
        _userService = userService;
        _userManager = userManager;
    }
    // SuperAdmin can only get Admins and Members; Admin can only read Members

    public async Task<Result<PagedList<UserDto>>> GetUsersAsync(PaginationParams p)
    {
        var currentUserRole = _userService.GetCurrentUserRole();

        if (currentUserRole == null)
            return Result<PagedList<UserDto>>.Failure("Unauthorized");
        
        if (currentUserRole != UserRoles.SUPER_ADMIN && currentUserRole != UserRoles.ADMIN)
            return Result<PagedList<UserDto>>.Failure("Forbidden");

        // Filter at DB level based on role
        IQueryable<UserDto> query = from user in _appDbContext.Users
                                    join userRole in _appDbContext.UserRoles on user.Id equals userRole.UserId
                                    join role in _appDbContext.Roles on userRole.RoleId equals role.Id
                                    where (currentUserRole == UserRoles.SUPER_ADMIN && (role.Name == UserRoles.ADMIN || role.Name == UserRoles.MEMBER)) || (currentUserRole == UserRoles.ADMIN && role.Name == UserRoles.MEMBER)
                                    select new UserDto(
                                        user.UserName!,
                                        user.FirstName,
                                        user.LastName,
                                        user.Email!,
                                        user.PhoneNumber,
                                        user.Country,
                                        user.EmailConfirmed,
                                        user.DateOfBirth,
                                        user.CreatedDate,
                                        role.Name!
                                    );

        var users = await PagedList<UserDto>.CreateAsync(query, p.Page, p.PageSize);


        if (users.TotalCount == 0)
            return Result<PagedList<UserDto>>.Failure("Users not found");

        return Result<PagedList<UserDto>>.Success(users);
    }
    public async Task<Result<string>> AssignToAdminRole(string userName)
    {
        return await AssignRole(userName, UserRoles.ADMIN);
    }

    public async Task<Result<string>> AssignToMemberRole(string userName)
    {
        return await AssignRole(userName, UserRoles.MEMBER);
    }
    //only super admin can assign roles
    private async Task<Result<string>> AssignRole(string userName, string targetRole)
    {
        var currentUserRole = _userService.GetCurrentUserRole();

        if (currentUserRole == null)
            return Result<string>.Failure("Unauthorized");

        if (currentUserRole != UserRoles.SUPER_ADMIN)
            return Result<string>.Failure("Only SuperAdmins can change user roles");

        var user = await _userManager.FindByNameAsync(userName);

        if (user == null)
            return Result<string>.Failure("User not found");

        var userRoles = await _userManager.GetRolesAsync(user);

        if (userRoles.Contains(targetRole))
            return Result<string>.Failure($"User already has the {targetRole} role");

        // Remove all roles and assign the target role
        var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
      
        if (!removeResult.Succeeded)
            return Result<string>.Failure("Failed to update user roles. No changes were made.");
        
        var addResult = await _userManager.AddToRoleAsync(user, targetRole);

        if (!addResult.Succeeded)
        {
            var rollback = await _userManager.AddToRolesAsync(user, userRoles);

            if (rollback.Succeeded)
                return Result<string>.Failure("Role assignment failed. User roles were restored.");
            
            return Result<string>.Failure("Role assignment failed. User currently has no role.");

        }
        return Result<string>.Success($"User assigned to {targetRole} role successfully");

    }
}
