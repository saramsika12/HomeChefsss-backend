using HomeChefss.Data;

namespace HomeChefss.Services
{
	public class OrderPaymentService
	{
		private readonly AppDbContext _context;

		public OrderPaymentService(AppDbContext context)
		{
			_context = context;
		}

		public async Task ProcessChefEarningsAsync(int chefId, decimal orderAmount)
		{
			var chef = await _context.Chefs.FindAsync(chefId)
				?? throw new Exception("Chef not found");

			chef.TotalEarnings += orderAmount;

			if (chef.SecurityDepositBalance < chef.SecurityDepositAmount)
			{
				var remainingDeposit = chef.SecurityDepositAmount - chef.SecurityDepositBalance;
				var depositCut = Math.Min(orderAmount, remainingDeposit);

				chef.SecurityDepositBalance += depositCut;
				orderAmount -= depositCut;
			}

			chef.WithdrawableBalance += orderAmount;

			await _context.SaveChangesAsync();
		}
	}
}
