using AutoMapper;
using Vezeeta.Core;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Models;
using Vezeeta.Core.Services;
using Vezeeta.Core.Utilities;
namespace Vezeeta.Service
{
	public class DoctorService : IDoctorService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public DoctorService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}


		public async Task<string> AddDoctorAppointments(AppointmentsDto appointmentsDto, string appUserId)
		{

			var doctor = await _unitOfWork.DoctorRepo.GetDoctorByAppUserId(appUserId);

			var result = await _unitOfWork.DoctorRepo.HasAppointments(doctor.Id);

			if (result)
				return "Doctor you have already set your appointments!";

			doctor.Price = appointmentsDto.Price;

			_unitOfWork.DoctorRepo.AddDoctorPrice(doctor);


			List<Appointment> appointments = new List<Appointment>();
			ICollection<AppointmentTime> appointmentTimes = new List<AppointmentTime>();


			foreach (var day in appointmentsDto.Days)
			{


				foreach (var time in day.Times)
				{

					appointmentTimes.Add(new AppointmentTime { Time = time });
				}

				appointments.Add(new Appointment
				{
					DoctorId = doctor.Id,
					DayOfWeek = day.DayOfWeek
					,
					AppointmentTimes = appointmentTimes.ToList(),
				});

				appointmentTimes.Clear();


			}

			await _unitOfWork.DoctorRepo.AddAppointments(appointments);

			await _unitOfWork.Complete();

			return "";


		}


		public async Task<string> DeleteAppointmentTime(int timeId, string appUserId)
		{

			//checking if doctor has appointments with this time
			var validationResult = await ValidateData(timeId, appUserId);

			if (validationResult.Length > 0)
				return validationResult;

			var time = await _unitOfWork.DoctorRepo.FindTimeById(timeId);

			_unitOfWork.DoctorRepo.DeleteTime(time);

			await _unitOfWork.Complete();

			return "";

		}



		public async Task<IReadOnlyList<object>> GetAllBookings(SearchDto searchDto, string appUserId)
		{
			var doctor = await _unitOfWork.DoctorRepo.GetDoctorByAppUserId(appUserId);

			return await _unitOfWork.DoctorRepo.GetAllBookings(searchDto, doctor.Id);
		}



		public async Task<string> UpdateAppointmentTime(TimeDto timeDto, string appUserId)
		{

			//checking if doctor has appointments with this time
			var validationResult = await ValidateData(timeDto.Id, appUserId);

			if (validationResult.Length > 0)
				return validationResult;


			var time = await _unitOfWork.DoctorRepo.FindTimeById(timeDto.Id);

			if (time is null)
				return "Invalid Id";


			var result = await _unitOfWork.DoctorRepo.HasSameTimeOnSameDay(time.AppointmentId, timeDto.Time);

			if (result)
				return "You already have this time on the same day!";

			time.Time = timeDto.Time;

			_unitOfWork.DoctorRepo.UpdateTime(time);

			await _unitOfWork.Complete();

			return "";


		}



		public async Task<string> UpdateBookingStatus(int id, string appUserId)
		{

			var doctor = await _unitOfWork.DoctorRepo.GetDoctorByAppUserId(appUserId);

			var booking = await _unitOfWork.DoctorRepo.GetBookingById(id, doctor.Id);

			if (booking is null)
				return "Invalid Id";

			var time = await _unitOfWork.DoctorRepo.FindTimeById(booking.AppointmentTimeId);

			booking.BookingStatus = BookingStatus.Completed;

			time.IsBooked = false;

			_unitOfWork.DoctorRepo.confirmCheckUp(booking);

			_unitOfWork.DoctorRepo.UpdateTime(time);

			await _unitOfWork.Complete();

			return "";

		}



		private async Task<string> ValidateData(int timeId, string appUserId)
		{
			var doctor = await _unitOfWork.DoctorRepo.GetDoctorByAppUserId(appUserId);

			var ownThisTime = await _unitOfWork.DoctorRepo.HasThisTime(doctor.Id, timeId);

			if (!ownThisTime)
				return "you don't have this time doctor!";


			var gotBookings = await _unitOfWork.DoctorRepo.HasBookingsWithSpecificTime(timeId, doctor.Id);

			if (gotBookings)
				return "you have bookings with that time doctor you can't update it";

			return "";
		}
	}
}
