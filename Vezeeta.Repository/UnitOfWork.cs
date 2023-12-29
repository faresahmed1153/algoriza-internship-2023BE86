using Microsoft.Extensions.Configuration;
using Vezeeta.Core;
using Vezeeta.Core.Models;
using Vezeeta.Core.Models.Identity;
using Vezeeta.Core.Repositories;
using Vezeeta.Repository.Repositories;

namespace Vezeeta.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext _dbContext;

		public IDashBoardRepository DashBoardRepo { get; set; }

		public IDiscountCodeRepository<DiscountCode> DiscountCodeRepo { get; set; }

		public IManageDoctorRepository<Doctor> ManageDoctorRepo { get; set; }

		public IManagePatientRepository<ApplicationUser> ManagePatientRepo { get; set; }

		public IDoctorRepository DoctorRepo { get; set; }

		public IPatientRepository PatientRepo { get; set; }


		public UnitOfWork(AppDbContext dbContext, IConfiguration configuration)
		{
			_dbContext = dbContext;

			DashBoardRepo = new DashBoardRepository(_dbContext, configuration);

			DiscountCodeRepo = new DiscountCodeRepository<DiscountCode>(_dbContext);

			ManageDoctorRepo = new ManageDoctorRepository<Doctor>(_dbContext);

			ManagePatientRepo = new ManagePatientRepository<ApplicationUser>(_dbContext);

			DoctorRepo = new DoctorRepository(_dbContext);

			PatientRepo = new PatientRepository(_dbContext);
		}


		public async Task<int> Complete()

		=> await _dbContext.SaveChangesAsync();



		public async ValueTask DisposeAsync()

		=> await _dbContext.DisposeAsync();


	}
}
