namespace Vezeeta.Core.Repositories
{
	public interface IGetByIdGeneric<T> where T : class
	{
		Task<T> GetByIdAsync(int id);
	}
}
