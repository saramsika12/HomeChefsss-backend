using HomeChefss.DTO.FoodItem;
using HomeChefss.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeChefss.Controllers
{
	[Route("api/[controller]")]
	[Authorize(Roles = "Chef")]
	[ApiController]
	public class ChefFoodController : ControllerBase
	{
		private readonly IFoodItemServices _foodItemServices;
		private readonly IWebHostEnvironment _env;

		public ChefFoodController(IFoodItemServices foodItemServices, IWebHostEnvironment env)
		{
			_foodItemServices = foodItemServices;
			_env = env;
		}

		[HttpPost("upload-expiry-image")]
		public async Task<IActionResult> UploadExpiryImage(UploadExpiryImageDto dto)
		{
			if (dto.Image == null || dto.Image.Length == 0)
				return BadRequest("No image provided");


			var imageUrl = await SaveImageAsync(dto.Image);

			await _foodItemServices.RefreshExpiryImageAsync(dto.FoodItemId, imageUrl);

			return Ok("Expiry image uploaded. Waiting for admin approval");
		}

		private async Task<string> SaveImageAsync(IFormFile image)
		{
			if (image == null || image.Length == 0)
				throw new ArgumentException("Invalid image file");

			var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "expiry");
			if (!Directory.Exists(uploadsFolder))
				Directory.CreateDirectory(uploadsFolder);

			var uniqueFileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
			var filePath = Path.Combine(uploadsFolder, uniqueFileName);


			await using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await image.CopyToAsync(stream);
			}

			// Return the relative URL for saving in DB
			return $"/images/expiry/{uniqueFileName}";
		}
	}
}
