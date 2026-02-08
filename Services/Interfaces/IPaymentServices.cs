using HomeChefss.DTO.Payment;
using HomeChefss.Models;

namespace HomeChefss.Services.Interfaces
{
	public interface IPaymentServices
	{
		Task<string> CreatePaymentAsync(string userId, CreatePaymentDto dto);
		Task VerifyPaymentAsync(VerifyPaymentDto dto);

	}
}
