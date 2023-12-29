using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vezeeta.Core.Models;
using Vezeeta.Core.Repositories;

namespace Vezeeta.Repository.Repositories
{
	public class DiscountCodeRepository<T> : IDiscountCodeRepository<T> where T : DiscountCode
	{
		private readonly AppDbContext _dbContext;

		public DiscountCodeRepository(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task AddAsync(T entity)

			=> await _dbContext.Set<T>().AddAsync(entity);


		public void Delete(T entity)

			=> _dbContext.Set<T>().Remove(entity);


		public void Update(T entity)

		=> _dbContext.Set<T>().Update(entity);


		public async Task<T> GetByIdAsync(int id)

			=> await _dbContext.Set<T>().FindAsync(id);


		public async Task<bool> GetByNameAsync(Expression<Func<DiscountCode, bool>> criteria)

			=> await _dbContext.Set<T>().AnyAsync(criteria);

		//=> await _dbContext.Set<T>().AnyAsync(d => d.Code == code);


		public async Task<bool> AppliedInBookingAsync(Expression<Func<Booking, bool>> criteria)

			=> await _dbContext.Set<Booking>().AnyAsync(criteria);

		//=> await _dbContext.Set<Booking>().AnyAsync(d => d.DiscountCodeId == id);
		//Expression<Func<DiscountCode, bool>> criteria
	}
}
