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
public record ForgetPasswordDto(string Email);
public record ResetPasswordDto(
    string Email,
    string NewPassword,
    string Code
);
public record ConfirmEmailDto(string Code);
