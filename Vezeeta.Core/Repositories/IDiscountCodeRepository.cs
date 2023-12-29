using System.Linq.Expressions;
using Vezeeta.Core.Models;

namespace Vezeeta.Core.Repositories
{
	public interface IDiscountCodeRepository<T> : IGenericRepository<T>, IGetByIdGeneric<T> where T : DiscountCode
	{
		Task<bool> GetByNameAsync(Expression<Func<DiscountCode, bool>> criteria);

		Task<bool> AppliedInBookingAsync(Expression<Func<Booking, bool>> criteria);
	}
}
