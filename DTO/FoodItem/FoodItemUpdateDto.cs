namespace HomeChefss.DTO.FoodItem
{
	public class FoodItemUpdateDto
	{
		public string Name { get; set; } = null!;
		public decimal Price { get; set; }
		public bool IsAvailable { get; set; }
		//public int ChefId { get; set; }
		public int CategoryId { get; set; }
	}
}
