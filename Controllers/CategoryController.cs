using HomeChefss.DTO.Category;
using HomeChefss.Models;
using HomeChefss.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeChefss.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryServices _categoryServices;

		public CategoryController(ICategoryServices categoryServices)
		{
			_categoryServices = categoryServices;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var categories = await _categoryServices.GetAllCategoriesAsync();
			return Ok(categories);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var category = await _categoryServices.GetCategoryByIdAsync(id);

			if (category == null)
				return NotFound("Category not found");
			return Ok(category);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<IActionResult> Create(CategoryCreateDto dto)
		{
				await _categoryServices.AddCategoryAsync(dto);
				return StatusCode(StatusCodes.Status201Created, "Category added successfully");
		}

		[Authorize(Roles = "Admin")]
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, CategoryUpdateDto dto)
		{
			
				await _categoryServices.UpdateCategoryAsync(id, dto);
				return Ok("Category updated successfully");
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
				await _categoryServices.DeleteCategoryAsync(id);
				return Ok("Category deleted successfully");
		}

	}
}
