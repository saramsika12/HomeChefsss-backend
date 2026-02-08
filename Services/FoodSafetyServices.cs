using HomeChefss.Repositories.Interface;
using HomeChefss.Services.Interfaces;

namespace HomeChefss.Services
{
	public class FoodSafetyServices : IFoodSafetyServices
	{
		private readonly IFoodItemRepository _foodItemRepository;

		public FoodSafetyServices(IFoodItemRepository foodItemRepository)
		{
			_foodItemRepository = foodItemRepository;
		}

		public async Task RefershExpiryImageAsync(int foodItemId, string imageUrl)
		{
			var foodItem = await _foodItemRepository.GetByIdAsync(foodItemId)
				?? throw new Exception("Food item not found");

			foodItem.ExpiryImageUrl = imageUrl;
			foodItem.ExpiryImageUploadAt = DateTime.UtcNow;

			await _foodItemRepository.UpdateAsync(foodItem);
		}
	}
}
