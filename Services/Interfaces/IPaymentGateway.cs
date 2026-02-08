using HomeChefss.Models;

namespace HomeChefss.Services.Interfaces
{
	public interface IPaymentGateway
	{
		Task<string> InitiateAsync(Payment payment);
		Task<bool> VerifyAsync(Payment payment, string transcationRef);
	}
}
