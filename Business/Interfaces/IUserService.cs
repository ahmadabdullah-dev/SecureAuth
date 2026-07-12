namespace Business.Interfaces;

public interface IUserService
{
    string? GetCurrentUserId();
    Task<Result<UserDto>> CurrentUserAsync();
    Task<Result<string>> RequestUpdateEmailAsync(RequestUpdateEmailDTO dto);
    Task<Result<string>> UpdateEmailAsync(UpdateEmailDTO dto);


}
