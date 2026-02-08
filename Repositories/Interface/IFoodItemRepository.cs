using HomeChefss.Models;

namespace HomeChefss.Repositories.Interface
{
	public interface IFoodItemRepository
	{
		Task<IEnumerable<FoodItem>> GetAllAsync();
		Task<IEnumerable<FoodItem>> GetByCategoryAsync(int categoryId);
		Task<FoodItem?> GetByIdAsync(int id);
		Task AddAsync(FoodItem foodItem);
		Task UpdateAsync(FoodItem foodItem);
		Task DeleteAsync(FoodItem foodItem);
		Task<int> CountByChefAsync(int chefId);
		Task<List<FoodItem>> GetExpiredFoodItemAsync();
	}
}
