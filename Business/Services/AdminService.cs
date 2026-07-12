using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class AdminService : IAdminService
{
    private readonly ApplicationDbContext _appDbContext;
    private readonly IUserService _userService;

    private const string SUPER_ADMIN_ROLE = "SuperAdmin";
    private const string ADMIN_ROLE = "Admin";
    private const string MEMBER_ROLE = "Member";

    public AdminService(
        ApplicationDbContext appDbContext, 
        IUserService userService)
    {
        _appDbContext = appDbContext;
        _userService = userService;
    }
    // SuperAdmin can only get Admins and Members; Admin can only read Members

    public async Task<Result<PagedList<UserDto>>> GetUsersAsync(PaginationParams p)
    {
        var currentUserRole = _userService.GetCurrentUserRole();

        if (currentUserRole == null)
            return Result<PagedList<UserDto>>.Failure("Unauthorized");
        if (currentUserRole != SUPER_ADMIN_ROLE && currentUserRole != ADMIN_ROLE)
            return Result<PagedList<UserDto>>.Failure("Forbidden");

        // Filter at DB level based on role
        IQueryable<UserDto> query = from user in _appDbContext.Users
                                    join userRole in _appDbContext.UserRoles on user.Id equals userRole.UserId
                                    join role in _appDbContext.Roles on userRole.RoleId equals role.Id
                                    where (currentUserRole == SUPER_ADMIN_ROLE && (role.Name == ADMIN_ROLE || role.Name == MEMBER_ROLE)) || (currentUserRole == ADMIN_ROLE && role.Name == MEMBER_ROLE)
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
}
