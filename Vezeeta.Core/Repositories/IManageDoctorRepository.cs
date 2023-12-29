using System.Linq.Expressions;
using Vezeeta.Core.Models;
using Vezeeta.Core.Models.Identity;

namespace Vezeeta.Core.Repositories
{
	public interface IManageDoctorRepository<T> : IGenericRepository<T>, IGetByIdGeneric<T>, IGetAllGeneric<T> where T : Doctor
	{
		Task<Specialization> GetSpecializationByIdAsync(int id);

		Task<bool> HasBookingsAsync(Expression<Func<Booking, bool>> criteria);

		Task<ApplicationUser> GetAppUserDoctorByIdAsync(Expression<Func<ApplicationUser, Doctor>> criteria, Expression<Func<ApplicationUser, bool>> expression);


	}
}
