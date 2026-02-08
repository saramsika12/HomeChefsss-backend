using HomeChefss.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeChefss.Services.Background
{
	public class ChefTrialMonitorServices : BackgroundService
	{
		private readonly IServiceScopeFactory _scopeFactory;

		public ChefTrialMonitorServices(IServiceScopeFactory scopeFactory)
		{
			_scopeFactory = scopeFactory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using var scope = _scopeFactory.CreateScope();
				var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

				var chefsToLock = await db.Chefs
					.Where(c =>
					c.ChefLevel == 1 &&
					c.TrialEndsAt < DateTime.UtcNow &&
					c.SecurityDepositBalance < c.SecurityDepositAmount &&
					!c.IsLocked)
					.ToListAsync();

				foreach (var chef in chefsToLock)
				{
					chef.IsLocked = true;
				}

				await db.SaveChangesAsync();
				await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
			}
		}
	}
}
