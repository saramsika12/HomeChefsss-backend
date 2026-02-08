namespace HomeChefss.DTO.Chef
{
	public class ChefProfileDto
	{
		public int ChefId { get; set; }
		public string KitchenName { get; set; } = null!;
		public string City { get; set; } = null!;
		public string Area { get; set; } = null!;
		public bool IsVerified { get; set; }

		public string FullName { get; set; } = null!;
		public string Email { get; set; } = null!;
	}
}
