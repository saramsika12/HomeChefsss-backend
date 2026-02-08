namespace HomeChefss.Models
{
	public class Order
	{
		public int OrderId { get; set; }

		public string UserId { get; set; } = null!;
		public ApplicationUser? User { get; set; }

		public int ChefId { get; set; }
		public Chef Chef { get; set; } = null!;

		public decimal TotalAmount { get; set; }

		public OrderStatus Status { get; set; } = OrderStatus.Pending;

		// Delivery address (actual)
		public Location DeliveryLocation { get; set; } = null!;

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime? DeliveredAt { get; set; }

		public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
	}
}
