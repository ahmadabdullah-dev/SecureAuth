using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class DataSeeder
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public DataSeeder(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    public async Task Seed()
    {
        await SeedRoles();
        await SeedUsers();
    }
    public async Task SeedRoles()
    {
        var roles = new List<AppRole>()
        {
            new() {Name = "SuperAdmin", Description = "Level 1"},
            new() {Name = "Admin", Description = "Level 2" },
            new() {Name = "Member", Description = "Level 3"}
        };

        if (!await _roleManager.Roles.AnyAsync())
        {
            foreach (var role in roles)
            {
                await _roleManager.CreateAsync(role);
            }
        }
    }
    public async Task SeedUsers()
    {
        var users = new List<(AppUser user, string role)>()
        {
            (new() {UserName = "superadmin1", Email= "superadmin1@test.com", Country ="Portugal", EmailConfirmed = true},"SuperAdmin"),
            (new() {UserName = "admin1", Email = "admin1@test.com", Country ="Argentina", EmailConfirmed = true},"Admin"),
            (new() {UserName = "member1", Email = "member1@test.com", Country="Spain", EmailConfirmed = true},"Member"),
            (new() {UserName = "superadmin2", Email= "superadmin2@test.com", Country ="USA", EmailConfirmed = true},"SuperAdmin"),
            (new() {UserName = "admin2", Email = "admin2@test.com", Country ="USA", EmailConfirmed = true},"Admin"),
            (new() {UserName = "member2", Email = "member2@test.com", Country="Westeros", EmailConfirmed = true },"Member"),
        };

        
        foreach (var (user,role) in users)
        {
            var existingUser = await _userManager.FindByNameAsync(user.UserName!);
         
            if (existingUser == null)
            {
                var result = await _userManager.CreateAsync(user,"Pa$$w0rd");       
               
                if(result.Succeeded)
                    await _userManager.AddToRoleAsync(user, role);
            }

        }

    }
}
