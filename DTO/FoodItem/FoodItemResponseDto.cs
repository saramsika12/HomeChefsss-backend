namespace HomeChefss.DTO.FoodItem
{
	public class FoodItemResponseDto
	{
		public int FoodItemId { get; set; }
		public string Name { get; set; } = null!;
		public decimal Price { get; set; }
		public bool IsAvailable { get; set; }
		public int ChefId { get; set; }
		public int CategoryId { get; set; }
		public string CategoryName { get; set; } = null!;
	}
}
