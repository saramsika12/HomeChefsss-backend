using HomeChefss.DTO.FoodItem;
using HomeChefss.Models;
using HomeChefss.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HomeChefss.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class FoodItemController : ControllerBase
	{
		private readonly IFoodItemServices _foodItemService;

		public FoodItemController(IFoodItemServices foodItemService)
		{
			_foodItemService = foodItemService;
		}

		[AllowAnonymous]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<FoodItemResponseDto>>> GetAll()
		{
			var foodItems = await _foodItemService.GetAllAsync();

			var result = foodItems.Select(f => new FoodItemResponseDto
			{
				FoodItemId = f.FoodItemId,
				Name = f.Name,
				Price = f.Price,
				IsAvailable = f.IsAvailable,
				ChefId = f.ChefId,
				CategoryId = f.CategoryId,
				CategoryName = f.Category != null ? f.Category.Name : string.Empty
			});
			return Ok(result);
		}

		[HttpGet("category/{categoryId}")]
		public async Task<ActionResult<IEnumerable<FoodItem>>> GetByCategory(int categoryId)
		{
			try
			{
				var foodItems = await _foodItemService.GetByCategoryAsync(categoryId);

				var result = foodItems.Select(f => new FoodItemResponseDto
				{
					FoodItemId = f.FoodItemId,
					Name = f.Name,
					Price = f.Price,
					IsAvailable = f.IsAvailable,
					ChefId = f.ChefId,
					CategoryId = f.CategoryId,
					CategoryName = f.Category != null ? f.Category.Name : string.Empty
				});
				return Ok(result);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<FoodItemResponseDto>> GetById(int id)
		{
			var foodItem = await _foodItemService.GetByIdAsync(id);
			if (foodItem == null) return NotFound("Food item not found");

			var result = new FoodItemResponseDto
			{
				FoodItemId = foodItem.FoodItemId,
				Name = foodItem.Name,
				Price = foodItem.Price,
				IsAvailable = foodItem.IsAvailable,
				ChefId = foodItem.ChefId,
				CategoryId = foodItem.CategoryId,
				CategoryName = foodItem.Category != null ? foodItem.Category.Name : string.Empty
			};

			return Ok(result);
		}

		[Authorize(Roles = "Chef")]
		[HttpPost]
		public async Task<ActionResult> Add([FromBody] FoodItemCreateDto dto)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null)
				return Unauthorized();

			//try
			//{
			//	//var foodItem = new FoodItem
			//	//{
			//	//	Name = foodItemDto.Name,
			//	//	Price = foodItemDto.Price,
			//	//	IsAvailable = foodItemDto.IsAvailable,
			//	//	ChefId = foodItemDto.ChefId,
			//	//	CategoryId = foodItemDto.CategoryId
			//	//};

			//	//await _foodItemService.AddAsync(foodItem);

			//	//return CreatedAtAction(nameof(GetById), new {id = foodItem.FoodItemId}, new FoodItemResponseDto
			//	//{
			//	//	FoodItemId = foodItem.FoodItemId,
			//	//	Name = foodItem.Name,
			//	//	Price = foodItem.Price,
			//	//	IsAvailable = foodItem.IsAvailable,
			//	//	ChefId = foodItem.ChefId,
			//	//	CategoryId = foodItem.CategoryId,
			//	//	CategoryName = ""
			//	//});

			//	await _foodItemService.AddAsync(foodItemDto);

			//	return StatusCode(StatusCodes.Status201Created, "Food item created successfully");
			//}
			//catch (Exception ex)
			//{
			//	return BadRequest(ex.Message);
			//}

			await _foodItemService.AddAsync(dto, userId);

			return StatusCode(StatusCodes.Status201Created, "Food item created successfully");

		}

		[HttpPut("{id}")]
		public async Task<ActionResult> Update(int id, [FromBody] FoodItemUpdateDto foodItemDto)
		{
			if (foodItemDto == null)
				return BadRequest("Invalid food item cannot be null");

			//try
			//{
			//	//var foodItem = new FoodItem
			//	//{
			//	//	FoodItemId = id, // important!
			//	//	Name = foodItemDto.Name,
			//	//	Price = foodItemDto.Price,
			//	//	IsAvailable = foodItemDto.IsAvailable,
			//	//	ChefId = foodItemDto.ChefId,
			//	//	CategoryId = foodItemDto.CategoryId
			//	//};

			//	//await _foodItemService.UpdateAsync(foodItem);
			//	//return Ok("Food item updated successfully");

			//	await _foodItemService.UpdateAsync(id, foodItemDto);
			//	return Ok("Food item updated successfully");
			//}
			//catch (Exception ex)
			//{
			//	return BadRequest(ex.Message);
			//}

			await _foodItemService.UpdateAsync(id, foodItemDto);
			return Ok("Food item updated successfully");
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(int id)
		{
				await _foodItemService.DeleteAsync(id);
				return Ok("Food item deleted successfully");
		}
	}
}
