using HomeChefss.Models;

namespace HomeChefss.DTO.Payment
{
	public class CreatePaymentDto
	{
		public int OrderId { get; set; }
		public PaymentMethod Method { get; set; }
	}
}
