using Vezeeta.Core.Models;
using Vezeeta.Core.Models.Identity;
using Vezeeta.Core.Repositories;

namespace Vezeeta.Core
{
	public interface IUnitOfWork : IAsyncDisposable
	{

		public IDashBoardRepository DashBoardRepo { get; set; }

		public IDiscountCodeRepository<DiscountCode> DiscountCodeRepo { get; set; }

		public IManageDoctorRepository<Doctor> ManageDoctorRepo { get; set; }

		public IManagePatientRepository<ApplicationUser> ManagePatientRepo { get; set; }

		public IDoctorRepository DoctorRepo { get; set; }

		public IPatientRepository PatientRepo { get; set; }

		Task<int> Complete();
	}
}
