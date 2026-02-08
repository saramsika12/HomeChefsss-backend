using HomeChefss.Models;

namespace HomeChefss.DTO.Delivery
{
	public class DeliveryResponseDto
	{
		public int DeliveryId { get; set; }
		public DeliveryStatus Status { get; set; }
		public decimal DeliveryFee { get; set; }
		public string? DeliveryPersonId { get; set; }
	}
}
