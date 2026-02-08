using HomeChefss.DTO.Category;
using HomeChefss.Models;

namespace HomeChefss.Services.Interfaces
{
	public interface ICategoryServices
	{
		Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync();
		Task<CategoryResponseDto?> GetCategoryByIdAsync(int id);
		Task AddCategoryAsync(CategoryCreateDto dto);
		Task UpdateCategoryAsync(int id, CategoryUpdateDto dto);
		Task DeleteCategoryAsync(int id);
	}
}
