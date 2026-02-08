using HomeChefss.Models;

namespace HomeChefss.Repositories.Interface
{
	public interface IChefRepository
	{
		Task<Chef?> GetByUserIdAsync(string userId);
	}
}
