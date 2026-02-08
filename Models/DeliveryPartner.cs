namespace HomeChefss.Models
{
	public class DeliveryPartner
	{
		public int DeliveryPartnerId { get; set; }
		public string UserId { get; set; } = null!;

		public string CitizenshipNumber { get; set; } = null!;
		public string LicenseNumber { get; set; } = null!;
		public bool HasDeliveryBag	{ get; set; }

		public VerificationStatus VerificationStatus { get; set; }
		public bool IsActive { get; set; }
		public double Rating { get; set; }
	}
}
