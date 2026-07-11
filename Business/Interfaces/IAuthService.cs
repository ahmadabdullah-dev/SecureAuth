namespace Business.Interfaces;

public interface IAuthService
{
    Task<Result<string>> RegisterAsync(RegisterDto dto);
    Task<Result<string>> LoginAsync(LoginDto dto);
    Task<Result<string>> LogoutAsync();
    Task<Result<string>> ForgetPasswordAsync(ForgetPasswordDTO dto);
    Task<Result<string>> ResetPasswordAsync(ResetPasswordDTO dto);
    Task<Result<string>> ResendEmailConfirmationCodeAsync();

}
