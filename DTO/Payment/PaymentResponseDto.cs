namespace HomeChefss.DTO.Payment
{
	public class PaymentResponseDto
	{
		public bool Success { get; set; }
		public string Message { get; set; } = string.Empty;
		
		public int PaymentId { get; set; }
		public int OrderId { get; set; }

		public string? TranscationRef { get; set; }

		public string Status { get; set; } = "Pending";
	}
}
