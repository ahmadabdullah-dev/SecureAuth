namespace Business.Dtos;

public class UserDto
{
    public string UserName { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Country { get; set; }
    public bool EmailConfirmed { get; set; }
    public DateOnly? BirthDate { get; set; }
    public DateTime JoinedDate { get; set; }
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