namespace HomeChefss.Models
{
	public class Review
	{
		public int ReviewId { get; set; }

		public int OrderId { get; set; }
		public Order Order { get; set; } = null!;

		public string UserId { get; set; } = null!;
		public ApplicationUser User { get; set; } = null!;

		public int ChefId { get; set; }
		public Chef Chef { get; set; } = null!;

		public int Rating { get; set; } // 1–5
		public string Comment { get; set; } = null!;

		public DateTime CreatedAt { get; set; }
	}
}
