using HomeChefss.Models;
using HomeChefss.Services.Interfaces;

namespace HomeChefss.Services.Gateway
{
	public class EsewaGateway : IPaymentGateway
	{
		public Task<string> InitiateAsync(Payment payment)
		{
			return Task.FromResult($"ESEWA_TEST_{Guid.NewGuid()}");

		}

		public Task<bool> VerifyAsync(Payment payment, string transcationRef)
		{
			return Task.FromResult(true);
		}
	}
}
