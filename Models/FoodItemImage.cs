namespace HomeChefss.Models
{
	public class FoodItemImage
	{
		public int Id { get; set; }
		public int FoodItemId { get; set; }
		public string ImageUrl { get; set; } = null!;
	}
}
