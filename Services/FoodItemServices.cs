using HomeChefss.DTO.FoodItem;
using HomeChefss.Models;
using HomeChefss.Repositories.Interface;
using HomeChefss.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeChefss.Services
{
	public class FoodItemServices : IFoodItemServices
	{
		private readonly IFoodItemRepository _foodItemRepository;
		private readonly ICategoryRepository _categoryRepository;
		private readonly IChefRepository _chefRepository;
		private readonly INotificationService _notification;

		public FoodItemServices (IFoodItemRepository foodItemRepository, ICategoryRepository categoryRepository, IChefRepository chefRepository, INotificationService notification)
		{
			_foodItemRepository = foodItemRepository;
			_categoryRepository = categoryRepository;
			_chefRepository = chefRepository;
			_notification = notification;
		}

		public async Task AddAsync(FoodItemCreateDto dto, string userId)
		{
			var chef = await _chefRepository.GetByUserIdAsync(userId);

			if (chef == null)
				throw new Exception("Chef profile not found");

			//if (!chef.IsVerified)
			//	throw new UnauthorizedAccessException("Chef not verified");

			if (chef.ChefLevel == 0)
				throw new UnauthorizedAccessException("Chef not approved yet");

			if (chef.ChefLevel == 1)
			{
				var count = await _foodItemRepository
					.CountByChefAsync(chef.ChefId);
					//.ContinueWith(t => t.Result.Count(f => f.ChefId == chef.ChefId));

				if (count >= 5)
					throw new Exception("Trial chefs can add only 5 food items");
			}

			if (chef.ChefLevel == 1 && chef.TrialEndsAt < DateTime.UtcNow)
			{
				throw new UnauthorizedAccessException("Trial period expired. Please complete verification.");
			}


			if (dto == null) 
			   throw new ArgumentNullException(nameof(dto));

			if (string.IsNullOrWhiteSpace(dto.Name))
				throw new Exception("Food name is required");

			if (dto.Price <= 0)
				throw new Exception("Price must be greater than zero");

			var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
			if (category == null)
				throw new Exception("Category does not exist");

			var foodItem = new FoodItem
			{
				Name = dto.Name,
				Price = dto.Price,
				IsAvailable = dto.IsAvailable,
				CategoryId = dto.CategoryId,
				ChefId = chef.ChefId
			};

			await _foodItemRepository.AddAsync(foodItem);
		}

		public async Task CheckAndBlockExpiredFoodAsync()
		{
			var expiredFoods = await _foodItemRepository.GetExpiredFoodItemAsync();

			foreach (var food in expiredFoods)
			{
				food.IsVisibleToCustomers = false;
				food.IsAvailable = false;

				await _foodItemRepository.UpdateAsync(food);

				await _notification.NotifyChefAsync(
					food.ChefId,
					$"Your food item '{food.Name}' is hidden due to expired ingredient proof. Please upload a new expiry image.");
			}
		}

		public async Task DeleteAsync(int id)
		{
			var existingItem = await _foodItemRepository.GetByIdAsync(id);
			if (existingItem == null) throw new Exception("Food item not found");

			await _foodItemRepository.DeleteAsync(existingItem);
		}

		public async Task<IEnumerable<FoodItem>> GetAllAsync()
		{
			return await _foodItemRepository.GetAllAsync();
		}

		public async Task<IEnumerable<FoodItem>> GetByCategoryAsync(int categoryId)
		{
			if (categoryId <= 0) 
			
				throw new ArgumentException("Invalid category id");

				return await _foodItemRepository.GetByCategoryAsync(categoryId);
			
		}

		public async Task<FoodItem?> GetByIdAsync(int id)
		{
			if (id <= 0) throw new ArgumentException("Invalid food item id");
			return await _foodItemRepository.GetByIdAsync(id);
		}

		public async Task RefreshExpiryImageAsync(int foodItemId, string imageUrl)
		{
			var food = await _foodItemRepository.GetByIdAsync(foodItemId)
				?? throw new Exception("Food item not found");

			food.ExpiryImageUrl = imageUrl;
			food.ExpiryImageUploadAt = DateTime.UtcNow;
			food.IsApproved = false;
			food.IsVisibleToCustomers = false;

			await _foodItemRepository.UpdateAsync(food);
		}

		public async Task UpdateAsync(int id, FoodItemUpdateDto dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto));

			var existingItem = await _foodItemRepository.GetByIdAsync(id);
			if (existingItem == null) throw new Exception("Food item not found");

			if (string.IsNullOrWhiteSpace(dto.Name)) throw new Exception("Food name is required");

			if (dto.Price <= 0) throw new Exception("Price must be greater than zero");

			var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
			if (category == null) throw new Exception("Category does not exist");

			existingItem.Name = dto.Name;
			existingItem.Price = dto.Price;
			existingItem.IsAvailable = dto.IsAvailable;
			existingItem.CategoryId = dto.CategoryId;
			//existingItem.ChefId = dto.ChefId;

			await _foodItemRepository.UpdateAsync(existingItem);

		}
	}
	
}
