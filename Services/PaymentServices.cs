using HomeChefss.Data;
using HomeChefss.DTO.Payment;
using HomeChefss.Models;
using HomeChefss.Services.Gateway;
using HomeChefss.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeChefss.Services
{
	public class PaymentServices : IPaymentServices
	{
		private readonly AppDbContext _context;
		private readonly IServiceProvider _provider;

		public PaymentServices(AppDbContext context, IServiceProvider provider)
		{
			_context = context;
			_provider = provider;
		}

		public async Task<string> CreatePaymentAsync(string userId, CreatePaymentDto dto)
		{
			var order = await _context.Orders
				.FirstOrDefaultAsync(o => o.OrderId == dto.OrderId && o.UserId == userId)
				?? throw new Exception("Order not found");

			var payment = new Payment
			{
				OrderId = order.OrderId,
				Amount = order.TotalAmount,
				Method = dto.Method,
				Status = PaymentStatus.Pending
			};

			_context.Payments.Add(payment);
			await _context.SaveChangesAsync();

			var gateway = ResolveGateway(dto.Method);
			var refId = await gateway.InitiateAsync(payment);

			payment.TranscationRef = refId;
			await _context.SaveChangesAsync();

			return refId;
		}

		public async Task VerifyPaymentAsync(VerifyPaymentDto dto)
		{
			var payment = await _context.Payments
				.Include(p => p.Order)
				.FirstOrDefaultAsync(p => p.PaymentId == dto.PaymentId)
				?? throw new Exception("Payment not found");

			var gateway = ResolveGateway(payment.Method);
			var success = await gateway.VerifyAsync(payment, dto.TranscationRef);

			payment.Status = success ? PaymentStatus.Paid : PaymentStatus.Failed;

			if (success)
			{
				payment.Status = PaymentStatus.Paid;
				payment.Order.Status = OrderStatus.Paid;
			}
			else
			{
				payment.Status = PaymentStatus.Failed;
			}

			await _context.SaveChangesAsync();
		}

		private IPaymentGateway ResolveGateway(PaymentMethod method)
		{
			return method switch
			{
				PaymentMethod.Esewa => _provider.GetRequiredService<EsewaGateway>(),
				PaymentMethod.Khalti => _provider.GetRequiredService<KhaltiGateway>(),
				PaymentMethod.CreditCard => _provider.GetRequiredService<DummyCardGateway>(),
				_ => throw new Exception("Unsupported payment method")

			};

		}
	}
}
