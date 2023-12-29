using Vezeeta.Core.Dtos;

namespace Vezeeta.Core.Services
{
	public interface IDoctorService
	{
		Task<IReadOnlyList<object>> GetAllBookings(SearchDto searchDto, string appUserId);

		Task<string> UpdateBookingStatus(int id, string appUserId);

		Task<string> UpdateAppointmentTime(TimeDto timeDto, string appUserId);

		Task<string> DeleteAppointmentTime(int id, string appUserId);

		Task<string> AddDoctorAppointments(AppointmentsDto appointmentsDto, string appUserId);
	}
}
