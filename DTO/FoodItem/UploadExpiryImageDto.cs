namespace HomeChefss.DTO.FoodItem
{
	public class UploadExpiryImageDto
	{
		public int FoodItemId { get; set; }
		public IFormFile Image { get; set; } = null!;
	}
}
