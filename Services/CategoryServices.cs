using HomeChefss.DTO.Category;
using HomeChefss.Models;
using HomeChefss.Repositories.Interface;
using HomeChefss.Services.Interfaces;

namespace HomeChefss.Services
{
	public class CategoryServices : ICategoryServices
	{
		private readonly ICategoryRepository _categoryRepository;

		public CategoryServices(ICategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync()
		{
			var categories = await _categoryRepository.GetAllAsync();

			return categories.Select(c => new CategoryResponseDto
			{
				CategoryId = c.CategoryId,
				Name = c.Name,
				FoodItemCount = c.FoodItems.Count
			});
		}

		public async Task<CategoryResponseDto?> GetCategoryByIdAsync(int id)
		{
			var category = await _categoryRepository.GetByIdAsync(id);

			if (category == null)
				throw new KeyNotFoundException("Category not found");

			return new CategoryResponseDto
			{
				CategoryId = category.CategoryId,
				Name = category.Name,
				FoodItemCount = category.FoodItems.Count
			};
		}

		public async Task AddCategoryAsync(CategoryCreateDto dto)
		{
			var existingCategories = await _categoryRepository.ExistsByNameAsync(dto.Name);

			if (existingCategories)
				throw new Exception("Category already exists");

			var category = new Category
			{
				Name = dto.Name,
				FoodItems = new List<FoodItem>()
			};

			await _categoryRepository.AddAsync(category);
		}

		public async Task UpdateCategoryAsync(int id, CategoryUpdateDto dto)
		{
			var category = await _categoryRepository.GetByIdAsync(id);

			if (category == null)
				throw new Exception("Category not found");

			category.Name = dto.Name;

			await _categoryRepository.UpdateAsync(category);
		}

		public async Task DeleteCategoryAsync(int id)
		{
			var category = await _categoryRepository.GetByIdAsync(id);

			if (category == null)
				throw new Exception("Category not found");

			var hasFoodItems = await _categoryRepository.HasFoodItemAsync(id);

			if (hasFoodItems)
				throw new Exception("Cannot delete category with existing food items");

			await _categoryRepository.DeleteAsync(category);
		}
	}
}
	
