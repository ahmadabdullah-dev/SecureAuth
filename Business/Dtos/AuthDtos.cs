namespace Business.Dtos;
public record RegisterDto(
    string UserName,
    string Email,
    string Password
);
public record LoginDto(
    string Email,
    string Password,
    bool IsPersistence
);
public record ForgetPasswordDTO(string Email);
public record ResetPasswordDTO(
    string Email,
    string NewPassword,
    string Code
);