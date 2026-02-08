using HomeChefss.Data;
using HomeChefss.Services.Interfaces;

namespace HomeChefss.Services
{
	public class NotificationService : INotificationService
	{
		private readonly AppDbContext _context;

		public NotificationService(AppDbContext context)
		{
			_context = context;
		}

		public async Task NotifyAdminAsync(string message)
		{
			Console.WriteLine($"Notify Admin: {message}");
			await Task.CompletedTask;
		}

		public async Task NotifyChefAsync(int chefId, string message)
		{
			//var notification = new Notification
			//{
			//	ChefId = chefId,
			//	Message = message,
			//	CreatedAt = DateTime.UtcNow
			//};

			//_context.Add(notification);
			//await _context.SaveChangesAsync();

			Console.WriteLine($"Notify Chef {chefId}: {message}");

			await Task.CompletedTask;
		}
	}
}
