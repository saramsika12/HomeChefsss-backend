using HomeChefss.Models;

namespace HomeChefss.DTO.Order
{
	public class CreateOrderDto
	{
		public int ChefId { get; set; }
		public Location DeliveryLocation { get; set; } = null!;
		public List<CreateOrderItemDto> Items { get; set; } = new();
	}
}
