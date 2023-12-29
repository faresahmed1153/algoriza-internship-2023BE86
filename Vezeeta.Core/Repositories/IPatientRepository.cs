using System.Linq.Expressions;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Models;

namespace Vezeeta.Core.Repositories
{
	public interface IPatientRepository
	{
		void CancelBooking(Booking booking);

		Task<IReadOnlyList<object>> GetAllAsync(int page, int pageSize);

		Task<IReadOnlyList<object>> GetAllAsync(int page, int pageSize, Expression<Func<Doctor, bool>> Criteria);

		Task<IReadOnlyList<object>> GetAllAsync(int page, int pageSize, Expression<Func<Appointment, bool>> DayCriteria);

		Task<IReadOnlyList<object>> GetAllAsync(int page, int pageSize, Expression<Func<AppointmentTime, bool>> DayCriteria);

		Task<IReadOnlyList<object>> GetAllBookingsForSpecificPatient(string patientId);

		Task<Booking> FindBookingWithPatientId(int bookingId, string patientId);

		Task<bool> BookedBeforeWithSameId(int timeId, string patientId);

		Task<AppointmentTime> FindTimeOfBooking(int timeId, bool isBooked);

		Task<DoctorWithSpecificAppointmentToReturnDto> GetDoctorWithSpeciliazationByTime(int timeId);

		Task<DiscountCode> FindDiscountCodeByName(string discountCode);

		Task<bool> DiscountCodeUsedBefore(int discountCode, string patientID);

		Task<int> FindNumberOfBookingsOfPatient(string patientId);

		Task BookAppointment(Booking booking);

		void UpdateDoctorAppoitnmentTimeStatus(AppointmentTime appointmentTime);
	}
}
