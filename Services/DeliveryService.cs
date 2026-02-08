using HomeChefss.Data;
using HomeChefss.Models;
using HomeChefss.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeChefss.Services
{
	public class DeliveryService : IDeliveryService
	{
		private readonly AppDbContext _context;
		private readonly IDeliveryFareService _fareService;

		public DeliveryService(AppDbContext context, IDeliveryFareService fareService)
		{
			_context = context;
			_fareService = fareService;
		}

		public async Task AcceptDeliveryAsync(int deliveryId, string partnerId)
		{
			var delivery = await _context.Deliveries.FindAsync(deliveryId)
				?? throw new Exception("Delivery not found");

			if (delivery.Status != DeliveryStatus.Requested)
				throw new Exception("Delivery already accepted");

			if (delivery.ReadyAt < DateTime.UtcNow)
				throw new Exception("Pickup time already passed");

			delivery.DeliveryPersonId = partnerId;
			delivery.Status = DeliveryStatus.Accepted;
			delivery.AcceptedAt = DateTime.UtcNow;

			await _context.SaveChangesAsync();
		}

		public async Task<int> CreateDeliveryAsync(int orderId, DeliveryType type, DateTime readyAt, string chefUserId)
		{
			var order = await _context.Orders
				.Include(o => o.Chef)
				.FirstOrDefaultAsync(o => o.OrderId == orderId)
				?? throw new Exception("Order not found");

			if (order.Chef.UserId != chefUserId)
				throw new Exception("Unauthorized");

			if (order.Status != OrderStatus.Cooking)
				throw new Exception("Order must be accepted first");

			if (order.Chef == null)
				throw new Exception("Chef not assigned to order");

			//var isPaid = await _context.Payments
			//	.AnyAsync(p => p.OrderId == orderId && p.Status == PaymentStatus.Paid);
				
			//if (!isPaid)
			//	throw new Exception("Order payment not completed");

			//if (order.Status != OrderStatus.Cooking)
			//	throw new Exception("Order must be started to cooking before creating deliveries");

			var deliveryExists = await _context.Deliveries
				.AnyAsync(d => d.OrderId == orderId);

			if (deliveryExists)
				throw new Exception("Delivery already created");

			var fee = _fareService.CalculateFare(
				order.Chef.KitchenLocation,
				order.DeliveryLocation
				);

			var delivery = new Delivery
			{
				OrderId = orderId,
				Type = type,
				Status = type == DeliveryType.ChefSelf
				? DeliveryStatus.Accepted
				: DeliveryStatus.Requested,
				DeliveryFee = fee
			};

			_context.Deliveries.Add(delivery);
			await _context.SaveChangesAsync();

			return delivery.DeliveryId;
		}


		//public async Task<int> CreateDeliveryAsync(int orderId, int chefId, DeliveryType type)
		//{
		//	var order = await _context.Orders.FindAsync(orderId)
		//		?? throw new Exception("Order not found");

		//	var chef = await _context.Chefs.FindAsync(chefId)
		//		?? throw new Exception("Chef not found");

		//	var fee = _fareService.CalculateFare(
		//		chef.KitchenLocation,
		//		order.DeliveryLocation
		//	);

		//	var delivery = new Delivery
		//	{
		//		OrderId = orderId,
		//		Type = type,
		//		Status = type == DeliveryType.ChefSelf
		//			 ? DeliveryStatus.Accepted
		//			 : DeliveryStatus.Requested,
		//		DeliveryFee = fee
		//	};

		//	_context.Deliveries.Add(delivery);
		//	await _context.SaveChangesAsync();

		//	return delivery.DeliveryId;


		//}

		public async Task MarkDeliveredAsync(int deliveryId)
		{
			var delivery = await _context.Deliveries
				.Include(d => d.Order)
				.Include(d => d.Order.Chef)
				.FirstOrDefaultAsync(d => d.DeliveryId == deliveryId)
				?? throw new Exception("Delivery not found");

			if (delivery.Status != DeliveryStatus.PickedUp)
				throw new Exception("Delivery not picked up");

			delivery.Status = DeliveryStatus.Delivered;
			delivery.DeliveryDate = DateTime.UtcNow;

			delivery.Order.Status = OrderStatus.Delivered;
			delivery.Order.DeliveredAt = DateTime.UtcNow;

			var chef = delivery.Order.Chef;
			chef.TotalEarnings += delivery.Order.TotalAmount;
			chef.WithdrawableBalance += delivery.Order.TotalAmount;

			await _context.SaveChangesAsync();
		}

		public async Task MarkPickedUpAsync(int deliveryId)
		{
			var delivery = await _context.Deliveries
				.Include(d => d.Order)
				.FirstOrDefaultAsync(d => d.DeliveryId == deliveryId)
				?? throw new Exception("Delivery not found");

			if (delivery.Status != DeliveryStatus.Accepted)
				throw new Exception("Delivery not accepted");

			if (delivery.Order.Status != OrderStatus.ReadyForPickup)
				throw new Exception("Delivery is not ready for pickup");

			delivery.Status = DeliveryStatus.PickedUp;
			delivery.PickedAt = DateTime.UtcNow;

			delivery.Order.Status = OrderStatus.PickedUp;

			await _context.SaveChangesAsync();
		}
	}
}
