namespace Business.Interfaces;

public interface IUserService
{
    string? GetCurrentUserId();
    string? GetCurrentUserRole();
    Task<Result<UserDto>> CurrentUserAsync();
    Task<Result<string>> RequestUpdateCurrentEmailAsync(string newEmail);
    Task<Result<string>> UpdateCurrentEmailAsync(string code);
    Task<Result<string>> ResendUpdateEmailConfirmationCodeAsync();
    Task<Result<string>> UpdateCurrentUserAsync(UpdateCurrentUserDto dto);
    Task<Result<string>> UpdateUserNameAsync(UpdateUserNameDto dto);
    Task<Result<string>> DeleteCurrentUserAsync();
}

