using System.Linq.Expressions;
using Vezeeta.Core;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Models;
using Vezeeta.Core.Services;
using Vezeeta.Core.Utilities;
namespace Vezeeta.Service
{
	public class PatientService : IPatientService
	{
		private readonly IUnitOfWork _unitOfWork;


		public PatientService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}


		public async Task<string> BookAppointment(BookingDto bookingDto, string patientId)
		{
			var timeOfBooking = await _unitOfWork.PatientRepo.FindTimeOfBooking(bookingDto.TimeId, false);

			if (timeOfBooking is null)
				return "Invalid time Id";



			var isBooked = await _unitOfWork.PatientRepo.BookedBeforeWithSameId(bookingDto.TimeId, patientId);

			if (isBooked)
				return "You Already Have A booking with that time";


			var doctorData = await _unitOfWork.PatientRepo.GetDoctorWithSpeciliazationByTime(bookingDto.TimeId);


			if (bookingDto.DiscountCode is not null)
			{
				var discountCode = await _unitOfWork.PatientRepo.FindDiscountCodeByName(bookingDto.DiscountCode);

				if (discountCode is null)
					return "Invalid Discount Code";

				var isUsedBefore = await _unitOfWork.PatientRepo.DiscountCodeUsedBefore(discountCode.Id, patientId);

				if (isUsedBefore)
					return "DiscountCode Has Been Used Before";

				int numOfBookingsOfSpecificPatient = await _unitOfWork.PatientRepo.FindNumberOfBookingsOfPatient(patientId);

				if (numOfBookingsOfSpecificPatient < discountCode.BookingsCompleted)
					return "you are not allowed to use the discount code because you are not meeting minmum number of bookings";

				decimal finalPrice = 0;


				if (discountCode.Type == 0)
				{
					var discountValue = (doctorData.Doctor.Price * discountCode.Value) / 100;

					finalPrice = doctorData.Doctor.Price - discountValue;

					if (finalPrice < 0)
						finalPrice = 0;


					await _unitOfWork.PatientRepo.BookAppointment(new Booking
					{
						PatientId = patientId,
						DoctorId = doctorData.Doctor.Id,
						AppointmentTimeId = bookingDto.TimeId,
						DiscountCodeId = discountCode.Id,
						SpecializationId = doctorData.Specialization.Id,
						Price = doctorData.Doctor.Price,
						FinalPrice = finalPrice,
					});

					timeOfBooking.IsBooked = true;

					_unitOfWork.PatientRepo.UpdateDoctorAppoitnmentTimeStatus(timeOfBooking);

					await _unitOfWork.Complete();

					return "";
				}

				finalPrice = doctorData.Doctor.Price - discountCode.Value;
				if (finalPrice < 0)
					finalPrice = 0;


				await _unitOfWork.PatientRepo.BookAppointment(new Booking
				{
					PatientId = patientId,
					DoctorId = doctorData.Doctor.Id,
					AppointmentTimeId = bookingDto.TimeId,
					DiscountCodeId = discountCode.Id,
					SpecializationId = doctorData.Specialization.Id,
					Price = doctorData.Doctor.Price,
					FinalPrice = finalPrice,
				});

				timeOfBooking.IsBooked = true;

				_unitOfWork.PatientRepo.UpdateDoctorAppoitnmentTimeStatus(timeOfBooking);

				await _unitOfWork.Complete();

				return "";
			}


			await _unitOfWork.PatientRepo.BookAppointment(new Booking
			{
				PatientId = patientId,
				DoctorId = doctorData.Doctor.Id,
				AppointmentTimeId = bookingDto.TimeId,
				SpecializationId = doctorData.Specialization.Id,
				Price = doctorData.Doctor.Price,
				FinalPrice = 0,
			});

			timeOfBooking.IsBooked = true;

			_unitOfWork.PatientRepo.UpdateDoctorAppoitnmentTimeStatus(timeOfBooking);

			await _unitOfWork.Complete();
			return "";


		}


		public async Task<string> CancelBooking(int bookingId, string appUserId)
		{
			var booking = await _unitOfWork.PatientRepo.FindBookingWithPatientId(bookingId, appUserId);

			if (booking is null)
				return "Invalid Id";

			booking.BookingStatus = BookingStatus.Cancelled;

			_unitOfWork.PatientRepo.CancelBooking(booking);

			var timeOfBooking = await _unitOfWork.PatientRepo.FindTimeOfBooking(booking.AppointmentTimeId, true);

			if (timeOfBooking is null)
				return "Invalid Id";

			timeOfBooking.IsBooked = false;

			_unitOfWork.PatientRepo.UpdateDoctorAppoitnmentTimeStatus(timeOfBooking);

			await _unitOfWork.Complete();

			return "";
		}



		public async Task<IReadOnlyList<object>> GetAllBookings(string patientId)

			=> await _unitOfWork.PatientRepo.GetAllBookingsForSpecificPatient(patientId);



		public async Task<IReadOnlyList<object>> GetAllDoctors(SearchDto searchDto)
		{
			Expression<Func<Doctor, bool>> criteria = null;

			Expression<Func<Appointment, bool>> DayCriteria = null;

			Expression<Func<AppointmentTime, bool>> TimeCriteria = null;



			if (searchDto.Criteria is not null)

			{
				if (Enum.TryParse<DayOfWeek>(searchDto.Criteria, true, out var parsedDay))
				{

					DayCriteria = (d => d.DayOfWeek == parsedDay);

					return await _unitOfWork.PatientRepo.GetAllAsync(searchDto.Page, searchDto.PageSize, DayCriteria);
				}


				if (DateTime.TryParseExact(searchDto.Criteria, "HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime dateTime))
				{

					TimeOnly timeOnly = TimeOnly.FromDateTime(dateTime);

					TimeCriteria = (t => t.Time == timeOnly && t.IsBooked == false);

					return await _unitOfWork.PatientRepo.GetAllAsync(searchDto.Page, searchDto.PageSize, TimeCriteria);
				}



				criteria = (d => d.ApplicationUserDoctor.FullName.Contains(searchDto.Criteria)
						|| d.ApplicationUserDoctor.Email.Contains(searchDto.Criteria)
						|| d.ApplicationUserDoctor.PhoneNumber.Contains(searchDto.Criteria)
						|| d.ApplicationUserDoctor.Email.Contains(searchDto.Criteria)
						|| d.Specialization.Name.Contains(searchDto.Criteria));

				return await _unitOfWork.PatientRepo.GetAllAsync(searchDto.Page, searchDto.PageSize, criteria);

			}
			return await _unitOfWork.PatientRepo.GetAllAsync(searchDto.Page, searchDto.PageSize);
		}
	}

}