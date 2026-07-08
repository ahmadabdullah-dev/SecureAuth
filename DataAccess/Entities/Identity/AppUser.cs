using Microsoft.AspNetCore.Identity;

namespace DataAccess.Entities.Identity;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Country { get; set; } 
    public DateOnly? DateOfBirth { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
