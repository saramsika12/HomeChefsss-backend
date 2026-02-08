namespace HomeChefss.Models
{
	public class OrderItem
	{
		public int OrderItemId { get; set; }

		public int OrderId { get; set; }
		public Order Order { get; set; } = null!;

		public string ItemName { get; set; } = null!;
		public int FoodItemId { get; set; }
		public FoodItem FoodItem { get; set; } = null!;

		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; }
	}
}
