using HomeChefss.DTO.Payment;
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
	public class PaymentController : ControllerBase
	{
		private readonly IPaymentServices _paymentService;
		private readonly UserManager<ApplicationUser> _userManager;

		public PaymentController(IPaymentServices paymentService, UserManager<ApplicationUser> userManager)
		{
			_paymentService = paymentService;
			_userManager = userManager;
		}

		[Authorize(Roles = "User")]
		[HttpPost("initiate")]
		public async Task<IActionResult> Initiate(CreatePaymentDto dto)
		{
			var userId = _userManager.GetUserId(User)!;
			var refId = await _paymentService.CreatePaymentAsync(userId, dto);
			return Ok(new {TranscationRef = refId});
		}

		[Authorize(Roles = "User")]
		[HttpPost("Verify")]
		public async Task<IActionResult> Verify(VerifyPaymentDto dto)
		{
			await _paymentService.VerifyPaymentAsync(dto);
			return Ok("Payment verified successfully");
		}
	}
}
