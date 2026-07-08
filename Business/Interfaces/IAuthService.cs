namespace Business.Interfaces;

public interface IAuthService
{
    Task<Result<string>> RegisterUserAsync(RegisterUserDto dto);
    Task<Result<string>> LoginUserAsync(LoginUserDto dto);
    Task<Result<string>> LogoutUserAsync();
}
