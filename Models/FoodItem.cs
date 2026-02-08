namespace HomeChefss.Models
{
	public class FoodItem
	{
		public int FoodItemId { get; set; }

		public string Name { get; set; } = null!;
		public decimal Price { get; set; }
		public string? Description { get; set; }
		public bool IsAvailable { get; set; }

		public string? Ingredients { get; set; }

		public string? ExpiryImageUrl { get; set; }
		public DateTime? ExpiryImageUploadAt { get; set; }
		public DateTime? DetectedExpiryDate { get; set; }

		public bool IsApproved { get; set; }
		public bool IsVisibleToCustomers { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public int ChefId { get; set; }
		public Chef? Chef { get; set; }

		public int CategoryId { get; set; }
		public Category? Category { get; set; }
	}
}
