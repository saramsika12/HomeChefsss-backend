namespace HomeChefss.Models
{
	public class DeliveryPenalty
	{
		public int Id { get; set; }
		public int DeliveryId { get; set; }
		public string Reason { get; set; } = null!;
		public decimal FineAmount { get; set; }
	}
}
