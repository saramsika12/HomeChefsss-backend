using HomeChefss.Data;
using HomeChefss.DTO.Chef;
using HomeChefss.Models;
using HomeChefss.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeChefss.Services
{
	public class ChefServices: IChefServices
	{
		private readonly AppDbContext _context;

		public ChefServices(AppDbContext context)
		{
			_context = context;
		}

		public async Task<Chef> GetChefByUserIdAsync(string userId)
		{
			return await _context.Chefs
				.FirstOrDefaultAsync(c => c.UserId == userId)
				?? throw new Exception("Chef profile not found");
		}

		public async Task<Chef> GetMyChefProfileAsync(string userId)
		{
			return await _context.Chefs
				.Include(c => c.FoodItems)
				.FirstOrDefaultAsync(c => c.UserId == userId)
				?? throw new Exception("Chef profile not found");
		}

		public async Task<IEnumerable<Chef>> GetVerifiedChefsAsync()
		{
			return await _context.Chefs
				.Where(c => c.VerificationStatus == VerificationStatus.Approved)
				.Include(c => c.FoodItems)
				.ToListAsync();
		}

		public async Task PaySecurityDepositAsync(string userId)
		{
			var chef = await _context.Chefs
				.FirstOrDefaultAsync(c => c.UserId == userId)
				?? throw new Exception("Chef profile not found");

			var remaining = chef.SecurityDepositAmount - chef.SecurityDepositBalance;

			if (remaining <= 0)
				throw new Exception("Deposit already completed");


			chef.SecurityDepositBalance += remaining;
			chef.SecurityDepositPaid = true;
			chef.DepositPaidAt = DateTime.UtcNow;
			chef.IsLocked = false;

			await _context.SaveChangesAsync();
		}


		//public async Task VerifyChefAsync(int chefId)
		//{
		//	var chef = await _context.Chefs.FindAsync(chefId)
		//		?? throw new Exception("Chef not found");

		//	chef.IsVerified = true;
		//	await _context.SaveChangesAsync();
		//}

		public async Task UpdateChefProfileAsync(string userId, ChefUpdateDto dto)
		{
			var chef = await _context.Chefs
				.FirstOrDefaultAsync(c => c.UserId == userId)
				?? throw new Exception("Chef profile not found");

			chef.KitchenName = dto.KitchenName;
			chef.City = dto.City;
			chef.Area = dto.Area;

			await _context.SaveChangesAsync();
		}
	}
}
