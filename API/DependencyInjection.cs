using Microsoft.AspNetCore.RateLimiting;

namespace API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPI(this IServiceCollection services)
    {
        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromDays(7);
            options.SlidingExpiration = true;
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowWeb",
                policy =>
                {
                    policy.WithOrigins("https://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
        });
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOrSuperAdmin", policy =>
            {
                policy.RequireRole("Admin", "SuperAdmin");
            });
        });
        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("rateLimiter", opt =>
            {
                opt.Window = TimeSpan.FromSeconds(10);
                opt.PermitLimit = 5;
                opt.QueueLimit = 0;
            });

            options.RejectionStatusCode = 429; 
        });
        return services;
    }
}
