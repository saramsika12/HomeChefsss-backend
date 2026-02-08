using HomeChefss.Data;
using HomeChefss.Models;
using HomeChefss.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HomeChefss.Repositories
{
	public class FoodItemRepository : IFoodItemRepository
	{
		private readonly AppDbContext _context;

		public FoodItemRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task AddAsync(FoodItem foodItem)
		{
			await _context.FoodItems.AddAsync(foodItem);
			await _context.SaveChangesAsync();
		}

		public async Task<int> CountByChefAsync(int chefId)
		{
			return await _context.FoodItems.CountAsync(f => f.ChefId == chefId);
		}

		public async Task DeleteAsync(FoodItem foodItem)
		{
			_context.FoodItems.Remove(foodItem);
			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<FoodItem>> GetAllAsync()
		{
			return await _context.FoodItems
				.Include(f => f.Category)
				//.Include(f => f.Chef)
				.ToListAsync();
		}

		public async Task<IEnumerable<FoodItem>> GetByCategoryAsync(int categoryId)
		{
			return await _context.FoodItems
				.Where(f => f.CategoryId == categoryId)
				.Include(f => f.Category)
				.ToListAsync();
		}

		public async Task<FoodItem?> GetByIdAsync(int id)
		{
			return await _context.FoodItems
				.Include(f => f.Category)
				//.Include(f => f.Chef)
				.FirstOrDefaultAsync(f => f.FoodItemId == id);
		}

		public async Task<List<FoodItem>> GetExpiredFoodItemAsync()
		{
			var threeMonthsAgo = DateTime.UtcNow.AddMonths(-3);

			return await _context.FoodItems
				.Where(f => (f.ExpiryImageUploadAt != null && f.ExpiryImageUploadAt < threeMonthsAgo) ||
				(f.DetectedExpiryDate != null && f.DetectedExpiryDate < DateTime.UtcNow)
				).ToListAsync();
		}

		public async Task UpdateAsync(FoodItem foodItem)
		{
			_context.FoodItems.Update(foodItem);
			await _context.SaveChangesAsync();
		}
	}
	
}
