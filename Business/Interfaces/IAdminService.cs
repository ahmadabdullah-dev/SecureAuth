using System.Security.Claims;

namespace Business.Interfaces;

public interface IAdminService
{
    Task<Result<PagedList<UserDto>>> GetUsersAsync(PaginationParams p);
    Task<Result<string>> AssignToAdminRole(AssignToAdminRoleDto dto);
    Task<Result<string>> AssignToMemberRole(AssignToMemberRoleDto username);
    Task<Result<string>> DeleteUserAsync(DeleteUserDto dto);

}
