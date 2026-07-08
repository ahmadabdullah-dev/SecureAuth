namespace Business.Dtos;
public record RegisterUserDto(
    string UserName,
    string Email,
    string Password
);
public record LoginUserDto(
    string Email,
    string Password,
    bool IsPersistence
);