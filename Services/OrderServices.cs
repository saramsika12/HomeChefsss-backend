using HomeChefss.Data;
using HomeChefss.DTO.Order;
using HomeChefss.Models;
using HomeChefss.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeChefss.Services
{
	public class OrderServices : IOrderServices
	{
		private readonly AppDbContext _context;

		public OrderServices(AppDbContext context)
		{
			_context = context;
		}

		public async Task AcceptOrderAsync(int orderId, string chefUserId)
		{
			var order = await _context.Orders
				.Include(o => o.Chef)
				.FirstOrDefaultAsync(o => o.OrderId == orderId);

			if (order == null)
				throw new Exception("Order not found");

			var isPaid = await _context.Payments
				.AnyAsync(p => p.OrderId == orderId && p.Status == PaymentStatus.Paid);

			if (!isPaid)
				throw new Exception("Order payment not completed");

			if (order.Status != OrderStatus.Paid)
				throw new Exception("Order must be paid before acceptance");


			if (order.Chef.UserId != chefUserId)
				throw new Exception("Unauthorized");

			order.Status = OrderStatus.Accepted;
			await _context.SaveChangesAsync();
		}

		public async Task<int> CreateOrderAsync(string userId, CreateOrderDto dto)
		{
			var chef = await _context.Chefs.FindAsync(dto.ChefId)
				?? throw new Exception("Chef not found");

			if (!dto.Items.Any())
				throw new Exception("Order must contain at least one item");

			var order = new Order
			{
				ChefId = chef.ChefId,
				UserId = userId,
				DeliveryLocation = dto.DeliveryLocation,
				Status = OrderStatus.Pending,
				CreatedAt = DateTime.UtcNow,
				Items = new List<OrderItem>()
			};

			decimal totalAmount = 0;

			foreach (var i in dto.Items)
			{
				var foodItem = await _context.FoodItems
					.FirstOrDefaultAsync(f => f.FoodItemId == i.FoodItemId && f.ChefId == chef.ChefId)
					?? throw new Exception("Invalid food item");


				var subTotal = foodItem.Price * i.Quantity;
				totalAmount += subTotal;

				order.Items.Add(new OrderItem
				{
					FoodItemId = foodItem.FoodItemId,
					ItemName = foodItem.Name,
					UnitPrice = foodItem.Price,
					Quantity = i.Quantity
				});
			}

			order.TotalAmount = totalAmount;

			_context.Orders.Add(order);
			await _context.SaveChangesAsync();


			return order.OrderId;
		}

		public async Task MarkDeliveredAsync(int orderId)
		{
			var order = await _context.Orders
				.Include(o => o.Chef)
				.FirstOrDefaultAsync(o => o.OrderId == orderId)
				?? throw new Exception("Order not found");

			if (order.Status != OrderStatus.PickedUp &&
				order.Status != OrderStatus.ReadyForPickup)
				throw new Exception("Order cannot be delivered");

			order.Status = OrderStatus.Delivered;
			order.DeliveredAt = DateTime.UtcNow;

			var chef = order.Chef;
			chef.TotalEarnings += order.TotalAmount;
			chef.WithdrawableBalance += order.TotalAmount;

			await _context.SaveChangesAsync();
		}

		public async Task MarkReadyForPickUpAsync(int orderId, string chefUserId)
		{
			var order = await _context.Orders
				.Include(o => o.Chef)
				.FirstOrDefaultAsync(o => o.OrderId == orderId)
				?? throw new Exception("Order not found");

			if (order.Chef.UserId != chefUserId)
				throw new Exception("Unauthorized");

			if (order.Status != OrderStatus.Cooking)
				throw new Exception("Order is not cooking");

			order.Status = OrderStatus.ReadyForPickup;
			await _context.SaveChangesAsync();
		}

		public async Task StartCookingAsync(int orderId, string chefUserId)
		{
			var order = await _context.Orders
				.Include(o => o.Chef)
				.FirstOrDefaultAsync(o => o.OrderId == orderId)
				?? throw new Exception("Order not found");

			if (order.Chef.UserId != chefUserId)
				throw new Exception("Unauthorized");

			if (order.Status != OrderStatus.Accepted)
				throw new Exception("Order must be accepted first");

			order.Status = OrderStatus.Cooking;
			await _context.SaveChangesAsync();
		}

		//public async Task DeliverOrderAsync(int orderId)
		//{
		//	var order = await _context.Orders
		//		.Include(o => o.Chef)
		//		//.ThenInclude(c => c.User)
		//		.FirstOrDefaultAsync(o => o.OrderId == orderId);

		//	//if (order.Status == OrderStatus.Delivered)
		//	//	throw new Exception("Order already delivered");

		//	if (order == null)
		//		throw new Exception("order not found"); 

		//	order.Status = OrderStatus.Delivered;
		//	order.DeliveredAt = DateTime.UtcNow;

		//	var chef = order.Chef;
		//	var amount = order.TotalAmount;

		//	if (chef.SecurityDepositBalance < chef.SecurityDepositAmount)
		//	{
		//		var remaining = chef.SecurityDepositAmount - chef.SecurityDepositBalance;
		//		var cut = Math.Min(amount, remaining);

		//		chef.SecurityDepositBalance += cut;
		//		amount -= cut;

		//		if (chef.SecurityDepositBalance >= chef.SecurityDepositAmount)
		//		{
		//			chef.SecurityDepositPaid = true;
		//		}
		//	}

		//	chef.WithdrawableBalance += amount;
		//	chef.TotalEarnings += order.TotalAmount;

		//	await _context.SaveChangesAsync();
		//}

	}
}
