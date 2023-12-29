using System.Linq.Expressions;
using Vezeeta.Core.Models;

namespace Vezeeta.Core.Repositories
{
	public interface IDashBoardRepository
	{

		Task<int> GetNumOfDoctors(Expression<Func<Doctor, bool>> criteria);

		Task<int> GetNumOfDoctors();



		Task<int> GetNumOfBookings(Expression<Func<Booking, bool>> criteria);

		Task<int> GetNumOfBookings();



		Task<IReadOnlyList<object>> GetTopFiveSpecializations(int top);



		Task<IReadOnlyList<object>> GetTopTenDoctors(int top);


	}
}
