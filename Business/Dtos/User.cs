namespace Business.Dtos;

public class UserDto
{
    public string Id { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; } = null!;
    public bool IsEmailConfirmed { get; set; }
    public string Role { get; set; } = null!;
}
public class UpdateCurrentUserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Country { get; set; }
    public DateOnly? DateOfBirth { get; set; }
}