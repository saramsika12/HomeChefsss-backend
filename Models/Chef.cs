using System.Text.Json.Serialization;

namespace HomeChefss.Models
{
	public class Chef
	{
		public int ChefId { get; set; }

		public string UserId { get; set; } = null!;
		public ApplicationUser User { get; set; } = null!;

		public string KitchenName { get; set; } = null!;
		public string City { get; set; } = null!;
		public string Area { get; set; } = null!;

		public Location KitchenLocation { get; set; } = null!;

		public string? KitchenImageUrl { get; set; }
		public string? HygieneCertificateUrl { get; set; }

		//  Verification & Trust
		public string? GovernmentIdNumber { get; set; }
		public string? GovernmentIdImageUrl { get; set; }
		public DateTime? GovernmentIdExpiryDate { get; set; }

		public string? CulinaryCertificateUrl { get; set; }
		public int YearsOfExperience { get; set; }

		public VerificationStatus VerificationStatus{ get; set; } = VerificationStatus.Pending;
		public bool IsVerified { get; set; }
		public int ChefLevel { get; set; }
		public DateTime? TrialEndsAt { get; set; }

		public bool SecurityDepositPaid { get; set; }
		public decimal SecurityDepositAmount { get; set; } = 5000;
		public decimal SecurityDepositBalance { get; set; } = 0;
		public DateTime? DepositPaidAt { get; set; }

		public decimal TotalEarnings {  get; set; }
		public decimal WithdrawableBalance { get; set; }

		public bool IsSuspended { get; set; }
		public int ActiveComplaints { get; set; }
		public string? AdminRemarks { get; set; }

		public bool IsLocked { get; set; }

		// Navigation
		[JsonIgnore]
		public List<FoodItem> FoodItems { get; set; } = new();
	}
}
