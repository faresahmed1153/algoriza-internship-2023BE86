using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vezeeta.Core.Models;
using Vezeeta.Core.Models.Identity;
using Vezeeta.Core.Repositories;

namespace Vezeeta.Repository.Repositories
{
	public class ManageDoctorRepository<T> : IManageDoctorRepository<T> where T : Doctor
	{
		private readonly AppDbContext _dbContext;

		public ManageDoctorRepository(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}


		public async Task AddAsync(T entity)

		=> await _dbContext.Set<T>().AddAsync(entity);


		public void Delete(T entity)

		=> _dbContext.Set<T>().Remove(entity);


		public async Task<IReadOnlyList<T>> GetAllAsync(int page, int pageSize, Expression<Func<T, bool>> Criteria)

		{
			if (Criteria is not null)
				return await _dbContext.Set<T>()
					.Include(d => d.ApplicationUserDoctor)
					.Include(d => d.Specialization)
					.Where(Criteria)
					.Skip((page - 1) * pageSize)
					.Take(pageSize)
					.ToListAsync();


			return await _dbContext.Set<T>()
					.Include(d => d.ApplicationUserDoctor)
					.Include(d => d.Specialization)
					.Skip((page - 1) * pageSize)
					.Take(pageSize)
					.ToListAsync();
		}



		public async Task<T> GetByIdAsync(int id)

			=> await _dbContext.Set<T>()
				.Include(d => d.ApplicationUserDoctor)
				.Include(d => d.Specialization)
				.FirstOrDefaultAsync(d => d.Id == id);




		public async void Update(T entity)

			=> _dbContext.Set<T>().Update(entity);


		public async Task<Specialization> GetSpecializationByIdAsync(int id)

		=> await _dbContext.Specializations.FindAsync(id);


		public async Task<bool> HasBookingsAsync(Expression<Func<Booking, bool>> criteria)

			=> await _dbContext.Bookings.AnyAsync(criteria);
		//=> await _dbContext.Bookings.AnyAsync(b => b.DoctorId == docId);

		public async Task<ApplicationUser> GetAppUserDoctorByIdAsync(Expression<Func<ApplicationUser, Doctor>> criteria, Expression<Func<ApplicationUser, bool>> expression)

		 => await _dbContext.Users.Include(criteria).FirstOrDefaultAsync(expression);

		//=> await _dbContext.Users.Include(d => d.Doctor).FirstOrDefaultAsync(d => d.Id == id);

	}
}
