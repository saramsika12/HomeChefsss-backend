namespace HomeChefss.Services.Interfaces
{
	public interface IFoodSafetyServices
	{
		Task RefershExpiryImageAsync(int foodItemId, string imageUrl);
	}
}
