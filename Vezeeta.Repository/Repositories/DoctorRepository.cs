using Microsoft.EntityFrameworkCore;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Models;
using Vezeeta.Core.Repositories;
using Vezeeta.Core.Utilities;

namespace Vezeeta.Repository.Repositories
{
	public class DoctorRepository : IDoctorRepository
	{
		private readonly AppDbContext _dbContext;

		public DoctorRepository(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}


		public async Task<Booking> GetBookingById(int bookingId, int doctorId)

		=> await _dbContext.Bookings
			.FirstOrDefaultAsync(b => b.Id == bookingId && b.BookingStatus == BookingStatus.Pending && b.DoctorId == doctorId);



		public void confirmCheckUp(Booking booking)

			=> _dbContext.Bookings.Update(booking);



		public async Task<Doctor> GetDoctorByAppUserId(string appUserId)

			=> await _dbContext.Doctors.FirstOrDefaultAsync(d => d.ApplicationUserId == appUserId);



		public async Task<bool> HasBookingsWithSpecificTime(int timeId, int doctorId)

			=> await _dbContext.Bookings.AnyAsync(b => b.AppointmentTimeId == timeId && b.DoctorId == doctorId);



		public async Task<bool> HasThisTime(int doctorId, int timeId)

			=> await _dbContext.AppointmentTimes
			.Include(t => t.Appointment)
			.AnyAsync(t => t.Id == timeId && t.Appointment.DoctorId == doctorId);



		public async Task<bool> HasAppointments(int doctorId)

			=> await _dbContext.Appointments.AnyAsync(a => a.DoctorId == doctorId);



		public async Task<bool> HasSameTimeOnSameDay(int appointmentId, TimeOnly timeOnly)

			=> await _dbContext.AppointmentTimes.AnyAsync(t => t.AppointmentId == appointmentId && t.Time == timeOnly);


		public void UpdateTime(AppointmentTime appointmentTime)

			=> _dbContext.AppointmentTimes.Update(appointmentTime);


		public void DeleteTime(AppointmentTime appointmentTime)
			=> _dbContext.AppointmentTimes.Remove(appointmentTime);


		public async Task<AppointmentTime> FindTimeById(int timeId)
			=> await _dbContext.AppointmentTimes.FindAsync(timeId);



		public async Task AddAppointments(List<Appointment> appointment)

			=> await _dbContext.Appointments.AddRangeAsync(appointment);



		public void AddDoctorPrice(Doctor doctor)
			=> _dbContext.Doctors.Update(doctor);


		public async Task<IReadOnlyList<object>> GetAllBookings(SearchDto searchDto, int doctorId)
		{

			if (searchDto.Criteria is not null)
			{
				// it will work because of validation of controller

				TimeOnly.TryParse(searchDto.Criteria, out TimeOnly convertedTime);

				return await _dbContext.Bookings
			.Where(b => b.DoctorId == doctorId)
			.Include(b => b.Patient)
			.Include(b => b.AppointmentTime)
			.ThenInclude(b => b.Appointment)
			.Where(b => b.AppointmentTime.Time == convertedTime)
			.Select(b => new
			{
				PictureUrl = b.Patient.PictureUrl,
				FullName = b.Patient.FullName,
				Email = b.Patient.Email,
				Phone = b.Patient.PhoneNumber,
				Gender = b.Patient.Gender,
				DateOfBirth = b.Patient.DateOfBirth,
			})
			.Skip((searchDto.Page - 1) * searchDto.PageSize)
			.Take(searchDto.PageSize)
			.ToListAsync();

			}


			else
				return await _dbContext.Bookings
						.Where(b => b.DoctorId == doctorId)
						.Include(b => b.Patient)
						.Select(b => new
						{
							PictureUrl = b.Patient.PictureUrl,
							FullName = b.Patient.FullName,
							Email = b.Patient.Email,
							Phone = b.Patient.PhoneNumber,
							Gender = b.Patient.Gender,
							DateOfBirth = b.Patient.DateOfBirth,
						})
						.Skip((searchDto.Page - 1) * searchDto.PageSize)
						.Take(searchDto.PageSize)
						.ToListAsync();
		}


	}
}
