namespace Business.Interfaces;

public interface IAdminService
{
    Task<Result<PagedList<UserDto>>> GetUsersAsync(PaginationParams p);
    Task<Result<string>> AssignToAdminRole(string username);
    Task<Result<string>> AssignToMemberRole(string username);
}
