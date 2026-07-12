namespace Business.Dtos;

public record UserDto(
    string UserName,
    string? FirstName,
    string? LastName,
    string Email,
    string? PhoneNumber,
    string? Country,
    bool EmailConfirmed,
    DateOnly? BirthDate,
    DateTime JoinedDate,
    IList<string> Roles
);
public record RequestUpdateEmailDto(
    string NewEmail
);
public record UpdateEmailDto(
    string Code
);
public record UpdateCurrentUserDto(
   string? FirstName,
   string? LastName,
   string? PhoneNumber,
   string? Country,
   DateOnly? DateOfBirth
);
public record UpdateUserNameDto(
    string NewUserName
);