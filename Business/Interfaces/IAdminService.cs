namespace Business.Interfaces;

public interface IAdminService
{
    Task<Result<PagedList<UserDto>>> GetUsersAsync(PaginationParams p);
}
