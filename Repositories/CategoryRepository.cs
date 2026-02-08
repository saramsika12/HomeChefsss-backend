using HomeChefss.Data;
using HomeChefss.Models;
using HomeChefss.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HomeChefss.Repositories
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly AppDbContext _context;

		public CategoryRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Category>> GetAllAsync()
		{
			return await _context.Categories
		.Include(c => c.FoodItems)
		.ToListAsync();

		}

		public async Task<Category?> GetByIdAsync(int id)
		{
			return await _context.Categories
		.Include(c => c.FoodItems)
		.FirstOrDefaultAsync(c => c.CategoryId == id);
		}

		public async Task AddAsync(Category category)
		{
			_context.Categories.Add(category);	
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(Category category)
		{
			_context.Categories.Update(category);
			await _context.SaveChangesAsync();
		}

		public async Task<bool> HasFoodItemAsync(int categoryId)
		{
			return await _context.FoodItems
				.AnyAsync(f => f.CategoryId == categoryId);
		}

		public async Task DeleteAsync(Category category)
		{
			_context.Categories.Remove(category);
			await _context.SaveChangesAsync();
		}

		public async Task<bool> ExistsByNameAsync(string name)
		{
			return await _context.Categories
				.AnyAsync(c => c.Name.ToLower() == name.ToLower());
		}
	}
}
