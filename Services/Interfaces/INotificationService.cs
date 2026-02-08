namespace HomeChefss.Services.Interfaces
{
	public interface INotificationService
	{
		Task NotifyChefAsync(int chefId, string message);
		Task NotifyAdminAsync(string message);
	}
}
