using HomeChefss.Models;

namespace HomeChefss.Repositories.Interface
{
	public interface ICategoryRepository
	{
		Task<IEnumerable<Category>> GetAllAsync();
		Task<Category?> GetByIdAsync(int id);
		Task AddAsync(Category category);
		Task UpdateAsync(Category category);
		Task<bool> ExistsByNameAsync(string name);
		Task<bool> HasFoodItemAsync(int categoryId);
		Task DeleteAsync(Category category);
	}
}
