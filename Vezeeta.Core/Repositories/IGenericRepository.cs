namespace Vezeeta.Core.Repositories
{
	public interface IGenericRepository<T> where T : class
	{
		Task AddAsync(T entity);

		void Update(T entity);

		void Delete(T entity);

	}
}
