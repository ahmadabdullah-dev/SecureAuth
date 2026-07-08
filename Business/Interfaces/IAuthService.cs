namespace Business.Interfaces;

public interface IAuthService
{
    Task<Result<string>> RegisterUserAsync(RegisterUserDto dto);
   
}
