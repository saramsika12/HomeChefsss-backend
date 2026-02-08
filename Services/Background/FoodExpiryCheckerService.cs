using HomeChefss.Data;
using HomeChefss.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeChefss.Services.Background
{
	public class FoodExpiryCheckerService : BackgroundService
	{
		private readonly IServiceScopeFactory _scopeFactory;
		private readonly ILogger<FoodExpiryCheckerService> _logger;

		public FoodExpiryCheckerService(
			IServiceScopeFactory scopeFactory,
			ILogger<FoodExpiryCheckerService> logger)
		{
			_scopeFactory = scopeFactory;
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Food Expiry Checker started");

			while (!stoppingToken.IsCancellationRequested)
			{
				await CheckExpiryFoodAsync();
				await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
			}
		}

		private async Task CheckExpiryFoodAsync()
		{
			using var scope = _scopeFactory.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
			var notifier = scope.ServiceProvider.GetRequiredService<INotificationService>();

			var now = DateTime.UtcNow;

			var expiredFoods = await context.FoodItems
			.Include(f => f.Chef)
			.Where(f =>
				f.IsVisibleToCustomers &&
				f.DetectedExpiryDate < now)
			.ToListAsync();

			foreach (var food in expiredFoods)
			{
				food.IsVisibleToCustomers = false;

				await notifier.NotifyChefAsync(
					food.ChefId,
					$"Food '{food.Name}' has expired. Upload a new expiry image to re-enable it. "
				);
			}

			if (expiredFoods.Any())
			{
				await context.SaveChangesAsync();
				_logger.LogInformation($"Disabled {expiredFoods.Count} expired food items");
			}
		}
	}
}
