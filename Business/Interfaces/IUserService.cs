namespace Business.Interfaces;

public interface IUserService
{
    string? GetCurrentUserId();
    string? GetCurrentUserRole();
    Task<Result<UserDto>> CurrentUserAsync();
    Task<Result<string>> RequestUpdateCurrentEmailAsync(string newEmail);
    Task<Result<string>> UpdateCurrentEmailAsync(string code);
    Task<Result<string>> ResendUpdateCurrentEmailConfirmationCodeAsync();
    Task<Result<string>> UpdateCurrentUserAsync(UpdateCurrentUserDto dto);
    Task<Result<string>> UpdateCurrentUserNameAsync(string userName);
    Task<Result<string>> DeleteCurrentUserAsync();
    Task<Result<UserDto>> GetUserByUserNameAsync(string userName);

}

