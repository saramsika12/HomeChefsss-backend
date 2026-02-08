namespace HomeChefss.Models
{
	public class Payment
	{
		public int PaymentId { get; set; }
		public int OrderId { get; set; }
		public Order Order { get; set; } = null!;
		public decimal Amount { get; set; }

		public PaymentMethod Method { get; set; }
		public PaymentStatus Status { get; set; }

		public string? TranscationRef { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	}
}
