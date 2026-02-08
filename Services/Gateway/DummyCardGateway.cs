using HomeChefss.Models;
using HomeChefss.Services.Interfaces;

namespace HomeChefss.Services.Gateway
{
	public class DummyCardGateway : IPaymentGateway
	{
		public Task<string> InitiateAsync(Payment payment)
		{
			return Task.FromResult($"CARD_TEST_{Guid.NewGuid()}");
		}

		public Task<bool> VerifyAsync(Payment payment, string transcationRef)
		{
			return Task.FromResult(true);
		}
	}
}
