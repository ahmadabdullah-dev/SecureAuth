namespace Business.Interfaces;

public interface IUserService
{
    string? GetCurrentUserId();
    Task<Result<UserDto>> CurrentUserAsync();
    Task<Result<string>> RequestUpdateEmailAsync(RequestUpdateEmailDto dto);
    Task<Result<string>> UpdateEmailAsync(UpdateEmailDto dto);
    Task<Result<string>> ResendUpdateEmailConfirmationCodeAsync();
    Task<Result<string>> UpdateCurrentUserAsync(UpdateCurrentUserDto dto);
    Task<Result<string>> UpdateUserNameAsync(UpdateUserNameDto dto);
    Task<Result<string>> DeleteCurrentUserAsync();
}

