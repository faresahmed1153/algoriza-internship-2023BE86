using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Models;
using Vezeeta.Core.Repositories;
using Vezeeta.Core.Utilities;

namespace Vezeeta.Repository.Repositories
{
	public class PatientRepository : IPatientRepository
	{
		private readonly AppDbContext _dbContext;

		public PatientRepository(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}


		public void CancelBooking(Booking booking)

			=> _dbContext.Bookings.Update(booking);


		public async Task<int> FindNumberOfBookingsOfPatient(string patientId)
			=> await _dbContext.Bookings.CountAsync(b => b.PatientId == patientId);


		public async Task<bool> DiscountCodeUsedBefore(int discountCode, string patientID)

			=> await _dbContext.Bookings.AnyAsync(b => b.DiscountCodeId == discountCode && b.PatientId == patientID);


		public async Task<Booking> FindBookingWithPatientId(int bookingId, string patientId)

			=> await _dbContext.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId && b.PatientId == patientId);


		public async Task<DiscountCode> FindDiscountCodeByName(string discountCode)
			=> await _dbContext.DiscountCodes.FirstOrDefaultAsync(d => d.Code == discountCode);


		public async Task<AppointmentTime> FindTimeOfBooking(int timeId, bool isBooked)

			=> await _dbContext.AppointmentTimes
			.FirstOrDefaultAsync(t => t.Id == timeId && t.IsBooked == isBooked);


		public async Task<DoctorWithSpecificAppointmentToReturnDto> GetDoctorWithSpeciliazationByTime(int timeId)

		  => await _dbContext.AppointmentTimes
				.Include(t => t.Appointment)
				.Include(t => t.Appointment.Doctor)
				.Include(t => t.Appointment.Doctor.Specialization)
			.Select(t => new DoctorWithSpecificAppointmentToReturnDto
			{
				AppointmentTime = t,
				Appointment = t.Appointment,
				Doctor = t.Appointment.Doctor,
				Specialization = t.Appointment.Doctor.Specialization
			})
				.FirstOrDefaultAsync(t => t.AppointmentTime.Id == timeId)
				;


		public async Task<bool> BookedBeforeWithSameId(int timeId, string patientId)
			=> await _dbContext.Bookings.AnyAsync(b => b.AppointmentTimeId == timeId && b.PatientId == patientId && b.BookingStatus == BookingStatus.Pending);


		public async Task<IReadOnlyList<object>> GetAllBookingsForSpecificPatient(string patientId)

			=> await _dbContext.Bookings
						.Where(b => b.PatientId == patientId)
				.Include(b => b.DiscountCode)
				.Include(b => b.AppointmentTime)
				.Include(b => b.AppointmentTime.Appointment)
				.Include(b => b.Doctor)
				.Include(b => b.Doctor.ApplicationUserDoctor)
				.Include(b => b.Doctor.Specialization)
				.Select(b => new
				{
					PictureUrl = b.Doctor.ApplicationUserDoctor.PictureUrl,
					FullName = b.Doctor.ApplicationUserDoctor.FullName,
					Email = b.Doctor.ApplicationUserDoctor.Email,
					PhoneNumber = b.Doctor.ApplicationUserDoctor.PhoneNumber,
					Specialize = b.Doctor.Specialization.Name,
					Price = b.Doctor.Price,
					Gender = b.Doctor.ApplicationUserDoctor.Gender.ToString(),
					Day = b.AppointmentTime.Appointment.DayOfWeek.ToString(),
					Time = b.AppointmentTime.Time,
					DiscountCode = b.DiscountCode.Code,
					FinalPrice = b.FinalPrice
				}).ToArrayAsync();


		public async Task BookAppointment(Booking booking)
			=> await _dbContext.Bookings.AddAsync(booking);

		public void UpdateDoctorAppoitnmentTimeStatus(AppointmentTime appointmentTime)
			=> _dbContext.AppointmentTimes.Update(appointmentTime);

		public async Task<IReadOnlyList<object>> GetAllAsync(int page, int pageSize)

				=> await _dbContext.Doctors
						.Include(d => d.ApplicationUserDoctor)
						.Include(d => d.Specialization)
						.Include(d => d.Appointments)
						.ThenInclude(d => d.AppointmentTimes)
						.Skip((page - 1) * pageSize)
						.Take(pageSize)
						.Select(d => new
						{
							PictureUrl = d.ApplicationUserDoctor.PictureUrl,
							FullName = d.ApplicationUserDoctor.FullName,
							Email = d.ApplicationUserDoctor.Email,
							PhoneNumber = d.ApplicationUserDoctor.PhoneNumber,
							Specialize = d.Specialization.Name,
							Price = d.Price,
							Gender = d.ApplicationUserDoctor.Gender.ToString(),
							Appointments = d.Appointments.Select(a => new
							{
								Day = a.DayOfWeek.ToString(),
								Times = a.AppointmentTimes.Where(t => t.IsBooked == false).Select(t => new
								{
									Id = t.Id,
									Time = t.Time,
								}).ToList()

							}).ToList()

						})
						.ToListAsync();


		public async Task<IReadOnlyList<object>> GetAllAsync(int page, int pageSize, Expression<Func<Doctor, bool>> Criteria)



			=> await _dbContext.Doctors
					.Include(d => d.ApplicationUserDoctor)
					.Include(d => d.Specialization)
					.Include(d => d.Appointments)
					.ThenInclude(a => a.AppointmentTimes)
					.Where(Criteria)
					.Skip((page - 1) * pageSize)
					.Take(pageSize)
					.Select(d => new
					{
						PictureUrl = d.ApplicationUserDoctor.PictureUrl,
						FullName = d.ApplicationUserDoctor.FullName,
						Email = d.ApplicationUserDoctor.Email,
						PhoneNumber = d.ApplicationUserDoctor.PhoneNumber,
						Specialize = d.Specialization.Name,
						Price = d.Price,
						Gender = d.ApplicationUserDoctor.Gender.ToString(),
						Appointments = d.Appointments.Select(a => new
						{
							Day = a.DayOfWeek.ToString(),
							Times = a.AppointmentTimes.Where(t => t.IsBooked == false).Select(t => new
							{
								Id = t.Id,
								Time = t.Time,
							}).ToList()

						}).ToList()

					})
					.ToListAsync();





		public async Task<IReadOnlyList<object>> GetAllAsync(int page, int pageSize, Expression<Func<Appointment, bool>> DayCriteria)
		{
			return await _dbContext.Appointments

					.Where(DayCriteria)
					.Include(a => a.AppointmentTimes)
					.Include(a => a.Doctor)
					.Include(a => a.Doctor.Specialization)
					.Include(a => a.Doctor.ApplicationUserDoctor)
					.Skip((page - 1) * pageSize)
					.Take(pageSize)
					.Select(a => new
					{
						PictureUrl = a.Doctor.ApplicationUserDoctor.PictureUrl,
						FullName = a.Doctor.ApplicationUserDoctor.FullName,
						Email = a.Doctor.ApplicationUserDoctor.Email,
						PhoneNumber = a.Doctor.ApplicationUserDoctor.PhoneNumber,
						Specialize = a.Doctor.Specialization.Name,
						Price = a.Doctor.Price,
						Gender = a.Doctor.ApplicationUserDoctor.Gender.ToString(),
						Appointments = new[]{
							new
							{
								Day = a.DayOfWeek.ToString(),
								Times = a.AppointmentTimes.Where(t => t.IsBooked == false).Select(t => new
								{
									Id = t.Id,
									Time = t.Time,
								}).ToList()
							}

						}

					})
					.ToListAsync();
		}




		public async Task<IReadOnlyList<object>> GetAllAsync(int page, int pageSize, Expression<Func<AppointmentTime, bool>> TimeCriteria)
		{
			return await _dbContext.AppointmentTimes

					.Where(TimeCriteria)
					.Include(a => a.Appointment)
					.Include(a => a.Appointment.Doctor)
					.Include(a => a.Appointment.Doctor.Specialization)
					.Include(a => a.Appointment.Doctor.ApplicationUserDoctor)
					.Skip((page - 1) * pageSize)
					.Take(pageSize)
					.Select(a => new
					{
						PictureUrl = a.Appointment.Doctor.ApplicationUserDoctor.PictureUrl,
						FullName = a.Appointment.Doctor.ApplicationUserDoctor.FullName,
						Email = a.Appointment.Doctor.ApplicationUserDoctor.Email,
						PhoneNumber = a.Appointment.Doctor.ApplicationUserDoctor.PhoneNumber,
						Specialize = a.Appointment.Doctor.Specialization.Name,
						Price = a.Appointment.Doctor.Price,
						Gender = a.Appointment.Doctor.ApplicationUserDoctor.Gender.ToString(),
						Appointments = new[]{
							new
							{
								Day = a.Appointment.DayOfWeek.ToString(),
								Times = a.Appointment.AppointmentTimes.Select(t => new
								{
									Id = t.Id,
									Time = t.Time,
								}).ToList()
							}

						}

					})
					.ToListAsync();
		}

	}
}
