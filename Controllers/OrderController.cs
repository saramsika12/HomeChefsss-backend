using HomeChefss.DTO.Order;
using HomeChefss.Models;
using HomeChefss.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeChefss.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly IOrderServices _orderServices;
		private readonly UserManager<ApplicationUser> _userManager;

		public OrderController(IOrderServices orderServices, UserManager<ApplicationUser> userManager)
		{
			_orderServices = orderServices;
			_userManager = userManager;
		}

		[Authorize(Roles = "User")]
		[HttpPost]
		public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
		{
			var userId = _userManager.GetUserId(User)!;

			var orderId = await _orderServices.CreateOrderAsync(userId, dto);
			return Ok(new { OrderId = orderId });
		}

		//[Authorize(Roles = "Admin, Chef")]
		//[HttpPut("{orderId}/deliver")]
		//public async Task<IActionResult> Deliver(int orderId)
		//{
		//	await _orderServices.DeliverOrderAsync(orderId);
		//	return Ok("Order delivered successfully");
		//}
		[Authorize(Roles = "Chef")]
		[HttpPut("{orderId}/accept")]
		public async Task<IActionResult> AcceptOrder(int orderId)
		{
			var chefUserId = _userManager.GetUserId(User)!;

			await _orderServices.AcceptOrderAsync(orderId, chefUserId);
			return Ok("Order accept");
		}

		[Authorize(Roles = "Chef")]
		[HttpPut("{orderId}/start-cooking")]
		public async Task<IActionResult> StartCooking(int orderId)
		{
			var chefUserId = _userManager.GetUserId(User)!;

			await _orderServices.StartCookingAsync(orderId, chefUserId);
			return Ok("Cooking started");
		}

		[Authorize(Roles = "Chef")]
		[HttpPut("{orderId}/ready")]
		public async Task<IActionResult> ReadyForPickup(int orderId)
		{
			var chefUserId = _userManager.GetUserId(User)!;

			await _orderServices.MarkReadyForPickUpAsync(orderId, chefUserId);
			return Ok("Order ready for pickup");
		}

		//[Authorize(Roles = "DeliveryPartner, Admin")]
		//[HttpPut("{orderId}/delivered")]
		//public async Task<IActionResult> MarkDelivered(int orderId)
		//{
		//	await _orderServices.MarkDeliveredAsync(orderId);
		//	return Ok("Order delivered successfully");
		//}

	}
}
