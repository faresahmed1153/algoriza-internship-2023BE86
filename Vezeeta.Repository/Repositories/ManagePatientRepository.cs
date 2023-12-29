using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vezeeta.Core.Models;
using Vezeeta.Core.Models.Identity;
using Vezeeta.Core.Repositories;
using Vezeeta.Core.Utilities;

namespace Vezeeta.Repository.Repositories
{
	public class ManagePatientRepository<T> : IManagePatientRepository<T> where T : ApplicationUser
	{
		private readonly AppDbContext _dbContext;

		public ManagePatientRepository(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IReadOnlyList<T>> GetAllAsync(int page, int pageSize, Expression<Func<T, bool>> Criteria)

		{
			if (Criteria is not null)
				return await _dbContext.Set<T>()
					.Where(p => p.Discriminator == Role.Patient)
					.Where(Criteria)
					.Skip((page - 1) * pageSize)
					.Take(pageSize)
					.ToListAsync();


			return await _dbContext.Set<T>()
					.Where(p => p.Discriminator == Role.Patient)
					.Skip((page - 1) * pageSize)
					.Take(pageSize)
					.ToListAsync();
		}


		public async Task<IReadOnlyList<Booking>> GetByIdAsync(string id)

			=> await _dbContext.Bookings
				.Where(p => p.PatientId == id)
				.Include(p => p.Patient)
				.Include(b => b.Specialization)
				.Include(b => b.DiscountCode)
				.Include(b => b.Doctor)
				.ThenInclude(b => b.ApplicationUserDoctor)
				.Include(b => b.AppointmentTime)
				.ThenInclude(b => b.Appointment)

				.ToListAsync();


	}
}
