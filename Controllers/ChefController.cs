using HomeChefss.DTO.Chef;
using HomeChefss.Models;
using HomeChefss.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeChefss.Controllers
{
	[Authorize(Roles = "Chef")]
	[Route("api/v1/[controller]")]
	[ApiController]
	public class ChefController : ControllerBase
	{
		private readonly IChefServices _chefService;
		private readonly UserManager<ApplicationUser> _userManager;

		public ChefController(IChefServices chefService, UserManager<ApplicationUser> userManager)
		{
			_chefService = chefService;
			_userManager = userManager;
		}

		[HttpGet("me")]
		public async Task<IActionResult> GetMyProfile()
		{
			var userId = _userManager.GetUserId(User);
			var chef = await _chefService.GetChefByUserIdAsync(userId);

			var lockResult = CheckChefLock(chef);
			if (lockResult != null) 
				return lockResult;

			if (chef == null)
				return NotFound("Chef profile not found");

			if (chef.VerificationStatus == VerificationStatus.Rejected)
				return Forbid("Chef application rejected");

			if (chef.IsSuspended)
				return Forbid("Chef account suspended");

			return Ok(new ChefResponseDto
				{
					ChefId = chef.ChefId,
					KitchenName = chef.KitchenName,
					City = chef.City,
					Area = chef.Area,
					YearsOfExperience = chef.YearsOfExperience,

					VerificationStatus = chef.VerificationStatus.ToString(),
					AdminRemarks = chef.AdminRemarks,

					ChefLevel = chef.ChefLevel,
					TrialEndsAt = chef.TrialEndsAt,

					SecurityDepositAmount = chef.SecurityDepositAmount,
					SecurityDepositPaid = chef.SecurityDepositPaid,

					IsLocked = chef.IsLocked

			});
		}


		[HttpPut("me")]
		public async Task<IActionResult> UpdateProfile(ChefUpdateDto dto)
		{
			var userId = _userManager.GetUserId(User);
			var chef = await _chefService.GetChefByUserIdAsync(userId);

			var lockResult = CheckChefLock(chef);
			if (lockResult != null) 
				return lockResult;

			await _chefService.UpdateChefProfileAsync(userId, dto);
			return Ok("Profile Updated");
		}

		[HttpPost("pay-deposit")]
		public async Task<IActionResult> PayDeposit()
		{
			var userId = _userManager.GetUserId(User)!;
			await _chefService.PaySecurityDepositAsync(userId);
			return Ok("Security deposit paid successfully");
		}

		private IActionResult? CheckChefLock(Chef chef)
		{
			if (chef.IsLocked)
			
				return Forbid("Account locked due to unpaid security deposit");

				if (chef.IsSuspended)
					return Forbid("Chef account suspended");

				return null;
			
		}
	}
}
