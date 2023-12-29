using AutoMapper;
using System.Linq.Expressions;
using Vezeeta.Core;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Models.Identity;
using Vezeeta.Core.Services;

namespace Vezeeta.Service
{
	public class ManagePatientService : IManagePatientService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ManagePatientService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<IReadOnlyList<PatientToReturnDto>> GetAllPatients(SearchDto searchDto)
		{

			Expression<Func<ApplicationUser, bool>> criteria = null;

			if (searchDto.Criteria is not null)
				criteria = (p => p.FullName.Contains(searchDto.Criteria)
							|| p.Email.Contains(searchDto.Criteria)
							|| p.PhoneNumber.Contains(searchDto.Criteria)
							|| p.Email.Contains(searchDto.Criteria));

			var patients = await _unitOfWork.ManagePatientRepo.GetAllAsync(searchDto.Page, searchDto.PageSize, criteria);


			var mappedPatients = patients.Select(p => new PatientToReturnDto
			{
				PictureUrl = p.PictureUrl,
				FullName = p.FullName,
				Email = p.Email,
				PhoneNumber = p.PhoneNumber,
				Gender = p.Gender,
				DateOfBirth = p.DateOfBirth,
			}).ToList();

			return mappedPatients;
		}


		public async Task<IReadOnlyList<object>> GetPatientById(string id)
		{
			var patientData = await _unitOfWork.ManagePatientRepo.GetByIdAsync(id);

			if (patientData.Count == 0)
				return patientData;


			List<object> mappedPatient = new List<object>();

			var details = new
			{


				patientData[0].Patient.PictureUrl,
				patientData[0].Patient.FullName,
				patientData[0].Patient.Email,
				patientData[0].Patient.PhoneNumber,
				patientData[0].Patient.Gender,
				patientData[0].Patient.DateOfBirth
			};
			mappedPatient.Add(details);
			var patientBookings = patientData.Select(b => new

			{
				PicturelUrl = b.Doctor.ApplicationUserDoctor.PictureUrl,
				DoctorName = b.Doctor.ApplicationUserDoctor.FullName,
				Specialize = b.Specialization.Name,
				Day = b.AppointmentTime.Appointment.DayOfWeek.ToString(),
				Time = b.AppointmentTime.Time,
				Price = b.Price,
				FinalPrice = b.FinalPrice,
				DiscountCode = b.DiscountCode?.Code,
				Status = b.BookingStatus.ToString()

			}
			).ToList();


			mappedPatient.Add(patientBookings);
			return mappedPatient;
		}
	}


}
