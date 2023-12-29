using Vezeeta.Core.Dtos;
using Vezeeta.Core.Models;

namespace Vezeeta.Core.Repositories
{
	public interface IDoctorRepository
	{

		void confirmCheckUp(Booking booking);

		Task<Doctor> GetDoctorByAppUserId(string doctorId);

		Task<Booking> GetBookingById(int bookingId, int doctorId);

		Task<bool> HasThisTime(int doctorId, int timeId);

		Task<bool> HasBookingsWithSpecificTime(int timeId, int doctorId);

		Task<AppointmentTime> FindTimeById(int timeId);

		void DeleteTime(AppointmentTime appointmentTime);

		void UpdateTime(AppointmentTime appointmentTime);

		Task<IReadOnlyList<object>> GetAllBookings(SearchDto searchDto, int doctorId);

		Task AddAppointments(List<Appointment> appointment);

		void AddDoctorPrice(Doctor doctor);

		Task<bool> HasAppointments(int doctorId);

		Task<bool> HasSameTimeOnSameDay(int appointmentId, TimeOnly timeOnly);

	}
}
