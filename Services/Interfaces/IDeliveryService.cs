using HomeChefss.Models;

namespace HomeChefss.Services.Interfaces
{
	public interface IDeliveryService
	{
		Task<int> CreateDeliveryAsync(int orderId, DeliveryType type, DateTime readyAt, string chefUserId);
		Task AcceptDeliveryAsync(int deliveryId, string partnerId);
		Task MarkPickedUpAsync(int deliveryId);
		Task MarkDeliveredAsync(int deliveryId);
	}
}
