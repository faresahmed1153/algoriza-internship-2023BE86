using System.Linq.Expressions;

namespace Vezeeta.Core.Repositories
{
	public interface IGetAllGeneric<T> where T : class
	{
		Task<IReadOnlyList<T>> GetAllAsync(int page, int pageSize, Expression<Func<T, bool>> Criteria);


	}
}
