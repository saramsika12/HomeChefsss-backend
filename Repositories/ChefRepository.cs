using HomeChefss.Data;
using HomeChefss.Models;
using HomeChefss.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HomeChefss.Repositories
{
	public class ChefRepository : IChefRepository
	{
		private readonly AppDbContext _context;

		public ChefRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<Chef?> GetByUserIdAsync(string userId)
		{
			return await _context.Chefs
				.FirstOrDefaultAsync(c => c.UserId == userId);
		}
	}
}
