using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using Vezeeta.Core.Models;
using Vezeeta.Core.Repositories;

namespace Vezeeta.Repository.Repositories
{
	public class DashBoardRepository : IDashBoardRepository
	{
		private readonly AppDbContext _dBContext;
		private readonly IConfiguration _configuration;


		public DashBoardRepository(AppDbContext appDbContext, IConfiguration configuration)
		{
			_dBContext = appDbContext;
			_configuration = configuration;
		}


		public async Task<int> GetNumOfBookings(Expression<Func<Booking, bool>> criteria)

		=> await _dBContext.Bookings.CountAsync(criteria);


		public async Task<int> GetNumOfBookings()

		=> await _dBContext.Bookings.CountAsync();




		public async Task<int> GetNumOfDoctors(Expression<Func<Doctor, bool>> criteria)

			=> await _dBContext.Doctors.CountAsync(criteria);


		public async Task<int> GetNumOfDoctors()

			=> await _dBContext.Doctors.CountAsync();



		public async Task<IReadOnlyList<object>> GetTopFiveSpecializations(int top)

			=> await _dBContext.Bookings.GroupBy(b => b.SpecializationId)
			.OrderByDescending(gp => gp.Count())
			.Take(top)
			.Select(gp => new
			{
				SpecializationId = gp.Key,
				count = gp.Count()
			})
			.Join(_dBContext.Specializations,
				b => b.SpecializationId,
				s => s.Id,
				(b, s) => new
				{

					specializationName = s.Name,
					count = b.count
				}
			).ToListAsync();



		public async Task<IReadOnlyList<object>> GetTopTenDoctors(int top)

			=> await _dBContext.Bookings.GroupBy(b => b.DoctorId)
					.OrderByDescending(gp => gp.Count())
					.Take(top)
					.Select(gp => new
					{
						DoctorId = gp.Key,
						totalOfBookings = gp.Count()
					})
					.Join(_dBContext.Doctors,
						b => b.DoctorId,
						d => d.Id,
						(b, d) => new
						{
							appUserId = d.ApplicationUserId,
							totalOfBookings = b.totalOfBookings,
							specialize = d.Specialization,

						}
						)
						.Join(_dBContext.Users,
							d => d.appUserId,
							u => u.Id,
							(d, u) => new
							{
								pictureUrl = u.PictureUrl,
								fullName = u.FullName,
								Specialize = d.specialize,
								totalOfBookings = d.totalOfBookings
							})
			.ToListAsync();
	}
}
