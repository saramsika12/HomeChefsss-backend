using HomeChefss.DTO.FoodItem;
using HomeChefss.Models;

namespace HomeChefss.Services.Interfaces
{
	public interface IFoodItemServices
	{
		Task<IEnumerable<FoodItem>> GetAllAsync();
		Task<IEnumerable<FoodItem>> GetByCategoryAsync(int categoryId);
		Task<FoodItem?> GetByIdAsync(int id);
		Task AddAsync(FoodItemCreateDto dto, string userId);
		Task CheckAndBlockExpiredFoodAsync();
		Task RefreshExpiryImageAsync(int foodItemId, string imageUrl);
		Task UpdateAsync(int id, FoodItemUpdateDto dto);
		Task DeleteAsync(int id);

	}
}
