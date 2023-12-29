using Vezeeta.Core.Dtos;

namespace Vezeeta.Core.Services
{
	public interface IPatientService
	{
		Task<IReadOnlyList<object>> GetAllDoctors(SearchDto searchDto);

		Task<string> BookAppointment(BookingDto bookingDto, string patientId);

		Task<string> CancelBooking(int bookingId, string patientId);

		Task<IReadOnlyList<object>> GetAllBookings(string patientId);




	}
}
