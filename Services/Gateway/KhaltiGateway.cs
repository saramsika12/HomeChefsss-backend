using HomeChefss.Models;
using HomeChefss.Services.Interfaces;

namespace HomeChefss.Services.Gateway
{
	public class KhaltiGateway : IPaymentGateway
	{
		public Task<string> InitiateAsync(Payment payment)
		{
			return Task.FromResult($"KHALTI_TEST_{Guid.NewGuid()}");
		}

		public Task<bool> VerifyAsync(Payment payment, string transcationRef)
		{
			return Task.FromResult(true);
		}
	}
}
