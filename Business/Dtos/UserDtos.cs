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
