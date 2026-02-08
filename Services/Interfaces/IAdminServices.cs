using HomeChefss.Models;

namespace HomeChefss.Services.Interfaces
{
	public interface IAdminServices
	{
		Task ApproveChefAsync(int chefId, string? remarks);
		Task FinalVerifyChefAsync(int chefId);
		Task RejectChefAsync(int chefId, string reason);
		Task SetSecurityDepositAsync(int chefId, decimal amount);
		Task<IEnumerable<Chef>> GetPendingChefsAsync();
		Task<IEnumerable<Chef>> GetTrialChefsAsync();
		Task<IEnumerable<Chef>> GetRejectedChefsAsync();

	}
}
