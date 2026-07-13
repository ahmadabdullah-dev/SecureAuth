using Business.Services.Background;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Business;

public static class DependencyInjection
{
    public static IServiceCollection AddBusiness(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAdminService,AdminService>();

        services.Configure<EmailConfiguration>(configuration.GetSection("EmailConfiguration"));
     
        services.AddHostedService<DeleteUsersWithUnconfirmedEmails>();

        return services;

    }
}
