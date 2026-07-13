using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Business.Services.Background;
// Background job that runs every minute and permanently deletes any user account that hasn't confirmed their email within 15 minutes of creation.
public class DeleteUsersWithUnconfirmedEmails : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<DeleteUsersWithUnconfirmedEmails> _logger;

    public DeleteUsersWithUnconfirmedEmails(IServiceProvider services, ILogger<DeleteUsersWithUnconfirmedEmails> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var cutOff = DateTime.UtcNow.AddMinutes(-15);

                await db.Users
                    .Where(u => !u.EmailConfirmed && u.CreatedDate < cutOff)
                    .ExecuteDeleteAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting users with unconfirmed emails.");
            }

            try
            {
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }
}