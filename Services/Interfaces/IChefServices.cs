using HomeChefss.DTO.Chef;
using HomeChefss.Models;

namespace HomeChefss.Services.Interfaces
{
	public interface IChefServices
	{
		Task<Chef> GetMyChefProfileAsync(string userId);
		Task<IEnumerable<Chef>> GetVerifiedChefsAsync();
		Task UpdateChefProfileAsync(string userId, ChefUpdateDto dto);
		Task PaySecurityDepositAsync(string userId);
		Task<Chef> GetChefByUserIdAsync(string userId);

	}
}
