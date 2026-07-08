using Microsoft.AspNetCore.Identity;

namespace DataAccess.Entities.Identity;

public class AppRole : IdentityRole
{
    public string? Description { get; set; }
}
