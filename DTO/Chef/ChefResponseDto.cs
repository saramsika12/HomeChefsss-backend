namespace HomeChefss.DTO.Chef
{
	public class ChefResponseDto
	{
		public int ChefId { get; set; }
		public string KitchenName { get; set; } = null!;
		public string City { get; set; } = null!;
		public string Area { get; set; } = null!;

		public string VerificationStatus { get; set; } = null!;
		public string? AdminRemarks { get; set; }

		public int YearsOfExperience { get; set; }

		public int ChefLevel { get; set; }
		public decimal SecurityDepositAmount { get; set; }
		public bool SecurityDepositPaid { get; set; }
		public DateTime? TrialEndsAt { get; set; }

		public bool IsLocked { get; set; }

	}
}
