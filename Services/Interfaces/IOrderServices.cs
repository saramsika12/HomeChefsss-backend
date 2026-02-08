using HomeChefss.DTO.Order;

namespace HomeChefss.Services.Interfaces
{
	public interface IOrderServices
	{
		Task<int> CreateOrderAsync (string userId, CreateOrderDto dto);

		//Task DeliverOrderAsync (int orderId);
		Task AcceptOrderAsync(int orderId, string chefUserId);
		Task StartCookingAsync(int orderId, string chefUserId);
		Task MarkReadyForPickUpAsync(int orderId, string chefUserId);
		Task MarkDeliveredAsync(int orderId);
	}
}
