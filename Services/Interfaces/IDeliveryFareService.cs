using HomeChefss.Models;

namespace HomeChefss.Services.Interfaces
{
	public interface IDeliveryFareService
	{
		decimal CalculateFare(Location from, Location to);
	}
}
