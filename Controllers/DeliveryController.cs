using HomeChefss.DTO.Delivery;
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
	public class DeliveryController : ControllerBase
	{
		private readonly IDeliveryService _service;
		private readonly UserManager<ApplicationUser> _userManager;

		public DeliveryController(IDeliveryService service,
			UserManager<ApplicationUser> userManager)
		{
			_service = service;
			_userManager = userManager;
		}

		[Authorize(Roles = "Chef")]
		[HttpPost]
		public async Task<IActionResult> Create(
			CreateDeliveryDto dto)
		{ 
			var chefUserId = _userManager.GetUserId(User)!;

			//var deliveryId = await _service.CreateDeliveryAsync(
			//	dto.OrderId,
			//	dto.Type,
			//	dto.ReadyAt,
			//	chefUserId
			//	);

			var deliveryId = await _service.CreateDeliveryAsync(
				dto.OrderId,
				dto.Type,
				dto.ReadyAt,
				chefUserId
				);

			return Ok(new {DeliveryId = deliveryId});
		}

		[Authorize(Roles = "DeliveryPartner")]
		[HttpPut("{deliveryId}/accept")]
		public async Task<IActionResult> Accept(int deliveryId)
		{
			var partnerId = _userManager.GetUserId(User)!;
			await _service.AcceptDeliveryAsync(deliveryId, partnerId);
			return Ok("Delivery accepted");
		}

		[Authorize(Roles = "DeliveryPartner")]
		[HttpPut("{deliveryId}/pickup")]
		public async Task<IActionResult> PickUp(int deliveryId)
		{
			await _service.MarkPickedUpAsync(deliveryId);
			return Ok("Order picked up");
		}

		[Authorize(Roles = "DeliveryPartner")]
		[HttpPut("{deliveryId}/deliver")]
		public async Task<IActionResult> Deliver(int deliveryId)
		{
			await _service.MarkDeliveredAsync(deliveryId);
			return Ok("Order delivered");
		}
	}
}
