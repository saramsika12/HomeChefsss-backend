using HomeChefss.Data;
using HomeChefss.DTO.Admin;
using HomeChefss.DTO.Delivery;
using HomeChefss.Models;
using HomeChefss.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeChefss.Controllers
{
	[Authorize(Roles = "Admin")]
	[Route("api/[controller]")]
	[ApiController]
	public class AdminController : ControllerBase
	{
		private readonly IAdminServices _adminServices;
		private readonly UserManager<ApplicationUser> _userManager;

		public AdminController(
			IAdminServices adminServices, 
			UserManager<ApplicationUser> userManager)
		{
			_adminServices = adminServices;
			_userManager = userManager;
		}


		//[HttpPut("verify-chef/{chefId}")]
		//public async Task<IActionResult> VerifyChef(int chefId, AdminChefVerificationDto dto)
		//{
		//	var chef = await _context.Chefs
		//		.Include(c => c.User)
		//        .FirstOrDefaultAsync(c => c.ChefId == chefId);

		//	if (chef == null)
		//		return NotFound("Chef not found");

		//	if (dto.IsApproved)
		//	{
		//		chef.VerificationStatus = VerificationStatus.Approved;
		//		chef.ChefLevel = 1; // TRIAL
		//		chef.TrialEndsAt = DateTime.UtcNow.AddDays(14);
		//		chef.IsVerified = false;

		//		if (await _userManager.IsInRoleAsync(chef.User, "ChefPending"))
		//			await _userManager.RemoveFromRoleAsync(chef.User, "ChefPending");

		//		//  ROLE CHANGE HAPPENS HERE
		//		if (!await _userManager.IsInRoleAsync(chef.User, "Chef"))
		//		{
		//			await _userManager.AddToRoleAsync(chef.User, "Chef");
		//		}
		//	}
		//	else
		//	{
		//		chef.VerificationStatus = VerificationStatus.Rejected;
		//		chef.ChefLevel = 0;
		//		chef.IsVerified = false;
		//	}

		//	chef.AdminRemarks = dto.AdminRemarks;

		//	await _context.SaveChangesAsync();

		//	return Ok("Chef verification processed");
		//}

		//[Authorize(Roles = "Admin")]
		//[HttpGet("users")]
		//public async Task<IActionResult> GetAllUsersAsync()
		//{
		//	var users = await _userManager.Users.ToListAsync();

		//	var result = new List<object>();

		//	foreach (var user in users)
		//	{
		//		result.Add(new
		//		{
		//			user.Id,
		//			user.Email,
		//			user.FullName,
		//			Roles = await _userManager.GetRolesAsync(user)
		//		});
		//	}

		//	return Ok(result);
		//}

		//[HttpGet("pending-chefs")]
		//public async Task<IActionResult> GetPendingChefs()
		//{
		//	var chefs = await _context.Chefs
		//		.Where(c => c.VerificationStatus == VerificationStatus.Pending
		//		|| c.VerificationStatus == VerificationStatus.UnderReview)
		//		.Include(c => c.User)
		//		.ToListAsync();

		//	return Ok(chefs);
		//}


		//[HttpGet("chef/{chefId}")]
		//public async Task<IActionResult> GetChefDetails(int chefId)
		//{
		//	var chef = await _context.Chefs
		//		.Include(c => c.User)
		//		.Include(c => c.FoodItems)
		//		.FirstOrDefaultAsync(c => c.ChefId == chefId);

		//	if (chef == null)
		//		return NotFound();

		//	return Ok(chef);
		//}

		[HttpPut("approve-chef/{chefId}")]
		public async Task<IActionResult> ApproveChef(int chefId, string? remarks)
		{
			await _adminServices.ApproveChefAsync(chefId, remarks);
			return Ok("Chef approve (trial started)");
		}

		[HttpPut("final-verify-chef/{chefId}")]
		public async Task<IActionResult> FinalVerifyChef(int chefId)
		{
			await _adminServices.FinalVerifyChefAsync(chefId);
			return Ok("Chef fully verified");
		}

		[HttpPut("reject-chef/{chefId}")]
		public async Task<IActionResult> RejectChef(int chefId, string reason)
		{
			await _adminServices.RejectChefAsync(chefId, reason);
			return Ok("Chef rejected");
		}

		[HttpPut("set-deposit/{chefId}")]
		public async Task<IActionResult> SetDeposit(int chefId, decimal amount)
		{
			await _adminServices.SetSecurityDepositAsync(chefId, amount);
			return Ok("Deposit set");
		}

		[HttpGet("pending-chefs")]
		public async Task<IActionResult> GetPendingChefs()
		{
			return Ok(await _adminServices.GetPendingChefsAsync());
		}

		[HttpGet("trial-chefs")]
		public async Task<IActionResult> GetTrialChefs()
		{
			return Ok(await _adminServices.GetTrialChefsAsync());
		}

		[HttpGet("rejected-chefs")]
		public async Task<IActionResult> GetRejectedChefs()
		{
			return Ok(await _adminServices.GetRejectedChefsAsync());
		}

		[HttpPost("approve-delivery-partner")]
		public async Task<IActionResult> ApprovedDeliveryPartner([FromBody] AssignDeliveryPartnerDto dto)
		{
			var user = await _userManager.FindByIdAsync(dto.UserId);
			if (user == null)
				return NotFound("User not found");

			if (await _userManager.IsInRoleAsync(user, "DeliveryPartner"))
				return BadRequest("User is already a DeliveryPartner");

			await _userManager.AddToRoleAsync(user, "DeliveryPartner");

			return Ok("User assigned as DeliveryPartner successfully");
		}

	}
}
