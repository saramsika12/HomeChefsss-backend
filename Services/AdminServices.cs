using HomeChefss.Data;
using HomeChefss.Models;
using HomeChefss.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeChefss.Services
{
	public class AdminServices : IAdminServices
	{
		private readonly AppDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public AdminServices(AppDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task ApproveChefAsync(int chefId, string? remarks)
		{
			var chef = await _context.Chefs
				.Include(c => c.User)
				.FirstOrDefaultAsync(c => c.ChefId == chefId)
				?? throw new Exception("Chef not found");

			chef.VerificationStatus = VerificationStatus.Approved;
			chef.ChefLevel = 1;
			chef.TrialEndsAt = DateTime.UtcNow.AddDays(14);
			chef.AdminRemarks = remarks;

			if (await _userManager.IsInRoleAsync(chef.User, "ChefPending"))
			{
				await _userManager.RemoveFromRoleAsync(chef.User, "ChefPending");
			}

			if (!await _userManager.IsInRoleAsync(chef.User, "Chef"))
			{
				await _userManager.AddToRoleAsync(chef.User, "Chef");
			}

			await _context.SaveChangesAsync();
		}

		public async Task FinalVerifyChefAsync(int chefId)
		{
			var chef = await _context.Chefs.FindAsync(chefId)
				?? throw new Exception("Chef not found");

			if (chef.SecurityDepositBalance < chef.SecurityDepositAmount)
				throw new InvalidOperationException("Security deposit not completed");

			chef.VerificationStatus = VerificationStatus.Verified;
			chef.ChefLevel = 2;
			chef.IsVerified = true;

			await _context.SaveChangesAsync();
		}

		public async Task RejectChefAsync(int chefId, string reason)
		{
			var chef = await _context.Chefs
				.Include(c => c.User)
				.FirstOrDefaultAsync(c => c.ChefId == chefId)
				?? throw new Exception("Chef not found");

			chef.VerificationStatus = VerificationStatus.Rejected;
			chef.ChefLevel = 0;
			chef.IsSuspended = true;
			chef.AdminRemarks = reason;

			await _userManager.RemoveFromRoleAsync(chef.User, "Chef");

			await _context.SaveChangesAsync();
		}

		public async Task SetSecurityDepositAsync(int chefId, decimal amount)
		{
			var chef = await _context.Chefs.FindAsync(chefId)
				?? throw new Exception("Chef not found");

			if (amount <= 0)
				throw new Exception("Deposit amount must be greater than zero");

			chef.SecurityDepositAmount = amount;

			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Chef>> GetPendingChefsAsync()
		{
			return await _context.Chefs
				.Where(c => c.VerificationStatus == VerificationStatus.Pending)
				.Include(c => c.User)
				.ToListAsync();
		}

		public async Task<IEnumerable<Chef>> GetTrialChefsAsync()
		{
			return await _context.Chefs
				.Where(c => c.ChefLevel == 1)
				.Include(c => c.User)
				.ToListAsync();
		}

		public async Task<IEnumerable<Chef>> GetRejectedChefsAsync()
		{
			return await _context.Chefs
				.Where(c => c.VerificationStatus == VerificationStatus.Rejected)
				.Include(c => c.User)
				.ToListAsync();
		}

	}
}
