namespace HomeChefss.Models
{
	public class Category
	{
		public int CategoryId { get; set; }
		public string Name { get; set; } = null!;

		public ICollection<FoodItem> FoodItems { get; set; } = new List<FoodItem>();
	}
}
