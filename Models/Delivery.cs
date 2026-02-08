namespace HomeChefss.Models
{
	public class Delivery
	{
		public int DeliveryId { get; set; }

		public int OrderId { get; set; }
		public Order Order { get; set; } = null!;

		public DeliveryType Type { get; set; }
		public DeliveryStatus Status { get; set; }

		public decimal DeliveryFee { get; set; }

		public string? DeliveryPersonId { get; set; }
		public ApplicationUser? DeliveryPerson { get; set; }

		public DateTime ReadyAt { get; set; }
		public DateTime? AcceptedAt { get; set; }

		public DateTime? PickedAt { get; set; }
		public DateTime? DeliveryDate { get; set; }
	}
}
