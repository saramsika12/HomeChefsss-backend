using Microsoft.AspNetCore.Identity;

namespace HomeChefss.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string FullName { get; set; } = null!;
		public string? City { get; set; }
		public string? Area { get; set; }
		public Chef? Chef { get; set; }
	}

}
