using HomeChefss.Models;

namespace HomeChefss.DTO.Delivery
{
	public class CreateDeliveryDto
	{
		public int OrderId { get; set; }
		public DeliveryType Type { get; set; }
		public DateTime ReadyAt { get; set; }
	}
}
