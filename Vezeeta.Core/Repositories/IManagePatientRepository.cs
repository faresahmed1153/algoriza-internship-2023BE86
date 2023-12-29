using Vezeeta.Core.Models;
using Vezeeta.Core.Models.Identity;

namespace Vezeeta.Core.Repositories
{
	public interface IManagePatientRepository<T> : IGetAllGeneric<T> where T : ApplicationUser
	{
		Task<IReadOnlyList<Booking>> GetByIdAsync(string id);


	}
}
