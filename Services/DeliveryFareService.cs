using HomeChefss.Models;
using HomeChefss.Services.Interfaces;

namespace HomeChefss.Services
{
	public class DeliveryFareService : IDeliveryFareService
	{
		public decimal CalculateFare(Location from, Location to)
		{
			var distance = CalculateDistanceKm(
				from.Latitude, from.Longitude, 
				to.Latitude, to.Longitude);

			if (distance <= 1)
				return 30;

			return 30 + (decimal)(distance - 1) * 15;
		}

		private double CalculateDistanceKm(
			double lat1, double lon1, 
			double lat2, double lon2)
		{
			const double R = 6371;

			var dLat = (lat2 - lat1) * Math.PI / 180;
			var dLon = (lon2 - lon1) * Math.PI / 180;

			var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
				Math.Cos(lat1 * Math.PI / 180) *
				Math.Cos(lat2 * Math.PI / 180) *
				Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

			var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
			return R * c;
		}
	}
}
