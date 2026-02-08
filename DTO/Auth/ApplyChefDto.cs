namespace HomeChefss.DTO.Auth
{
	public class ApplyChefDto
	{
		public string KitchenName { get; set; } = null!;
		public string City { get; set; } = null!;
		public string Area { get; set; } = null!;

		public string? GovernmentIdNumber { get; set; }
		public string? CulinaryCertificateUrl { get; set; }
		public int YearsOfExperience { get; set; }
	}
}
