using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using Vezeeta.Core;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Models;
using Vezeeta.Core.Models.Identity;
using Vezeeta.Core.Services;
using Vezeeta.Core.Utilities;
using Vezeeta.Service.Helpers;

namespace Vezeeta.Service
{
	public class ManageDoctorService : IManageDoctorService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IEmailService _emailService;
		private readonly IMapper _mapper;
		private readonly IConfiguration _configuration;

		public ManageDoctorService(

			IUnitOfWork unitOfWork,
			UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager,
			IEmailService emailService,
			IMapper mapper,
			IConfiguration configuration
			)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_roleManager = roleManager;
			_emailService = emailService;
			_mapper = mapper;
			_configuration = configuration;
		}

		public async Task<string> AddDoctor(DoctorDto doctorDto)
		{
			var speciailization = await _unitOfWork.ManageDoctorRepo.GetSpecializationByIdAsync(doctorDto.SpecializationId);

			if (speciailization is null)
				return "Invalid SpecializationId";

			var pictureName = await DocumentSettings.UploadFile(doctorDto.Picture, "DoctorImages");

			doctorDto.PictureUrl = string.Concat(_configuration["DoctorPathUrl"], pictureName);

			var mappedDoctor = _mapper.Map<DoctorDto, ApplicationUser>(doctorDto);

			mappedDoctor.UserName = doctorDto.Email.Split("@")[0];

			mappedDoctor.Discriminator = Role.Doctor;

			var creatingUser = await _userManager.CreateAsync(mappedDoctor, doctorDto.Password);

			if (!creatingUser.Succeeded)
				return "hmmm looks like creating doctor failed!";

			var doctor = await _userManager.FindByEmailAsync(doctorDto.Email);

			await _unitOfWork.ManageDoctorRepo.AddAsync(new Doctor
			{
				ApplicationUserId = doctor.Id,
				SpecializationId = doctorDto.SpecializationId,
			});


			var doctorRole = await _roleManager.FindByNameAsync(Role.Doctor);

			await _userManager.AddToRoleAsync(doctor, doctorRole.Name);


			await _unitOfWork.Complete();


			await _emailService.SendEmail(new EmailDto
			{
				To = doctorDto.Email,
				Subject = "credentials",
				Body = $"Your email is {doctorDto.Email} and password is {doctorDto.Password}"
			});

			return "";
		}

		public async Task<string> DeleteDoctor(int id)
		{
			var doctor = await _unitOfWork.ManageDoctorRepo.GetByIdAsync(id);

			if (doctor is null)
				return "Invalid doctor Id !";

			var hasBookings = await _unitOfWork.ManageDoctorRepo.HasBookingsAsync((Booking b)
				=> b.DoctorId == id);

			if (hasBookings)
				return "The doctor has bookings you can't make a delete operation";


			var fileName = doctor.ApplicationUserDoctor.PictureUrl.Split("DoctorImages/")[1];

			DocumentSettings.DeleteFile(fileName, "DoctorImages");


			var result = await _userManager.DeleteAsync(doctor.ApplicationUserDoctor);


			if (result.Succeeded)

				return "";


			return "hmmm looks like deleting doctor failed!";
		}

		public async Task<string> EditDoctor(DoctorUpdateDto doctorDto)
		{


			var doctor = await _unitOfWork.ManageDoctorRepo.GetByIdAsync(doctorDto.Id);


			if (doctor is null)
				return "Invalid Id";


			var phoneIsUsed = await _userManager.Users.AnyAsync(u => u.PhoneNumber == doctorDto.PhoneNumber);

			if (phoneIsUsed)
				return "phonenumber already used";


			var emailIsUsed = await _userManager.Users.AnyAsync(u => u.Email == doctorDto.Email);

			if (emailIsUsed)
				return "email already used";


			if (doctor.SpecializationId != doctorDto.SpecializationId)
			{
				var specialization = await _unitOfWork.ManageDoctorRepo.GetSpecializationByIdAsync(doctorDto.SpecializationId);

				if (specialization is null)
					return "Invalid Specialization Id";


				var hasBookings = await _unitOfWork.ManageDoctorRepo.HasBookingsAsync((Booking b)
				=> b.DoctorId == doctorDto.Id);


				if (hasBookings)
					return "The doctor has bookings with that specialization you can't update the specialization";


				doctor.SpecializationId = doctorDto.SpecializationId;

				_unitOfWork.ManageDoctorRepo.Update(doctor);

				await _unitOfWork.Complete();

			}

			if (doctorDto.Picture is not null)
			{
				var fileName = doctor.ApplicationUserDoctor.PictureUrl.Split("DoctorImages/")[1];

				DocumentSettings.DeleteFile(fileName, "DoctorImages");


				var pictureName = await DocumentSettings.UploadFile(doctorDto.Picture, "DoctorImages");

				doctorDto.PictureUrl = string.Concat(_configuration["DoctorPathUrl"], pictureName);
			}

			//since the data is in a navigational property and tracked by efcore 
			// i had to update it manually as automapper will make efcore ambiguous

			doctor.ApplicationUserDoctor.FullName = string.Concat(doctorDto.FirstName, doctorDto.LastName);
			doctor.ApplicationUserDoctor.PhoneNumber = doctorDto.PhoneNumber;
			doctor.ApplicationUserDoctor.Email = doctorDto.Email;
			doctor.ApplicationUserDoctor.UserName = doctorDto.Email.Split("@")[0];
			doctor.ApplicationUserDoctor.DateOfBirth = doctorDto.DateOfBirth;
			doctor.ApplicationUserDoctor.Gender = doctorDto.Gender;
			doctor.ApplicationUserDoctor.PictureUrl = doctorDto.PictureUrl;


			await _userManager.UpdateAsync(doctor.ApplicationUserDoctor);



			return "";
		}


		public async Task<IReadOnlyList<DoctorToReturnDto>> GetAllDoctors(SearchDto searchDto)
		{
			Expression<Func<Doctor, bool>> criteria = null;

			if (searchDto.Criteria is not null)
				criteria = (d => d.ApplicationUserDoctor.FullName.Contains(searchDto.Criteria)
							|| d.ApplicationUserDoctor.Email.Contains(searchDto.Criteria)
							|| d.ApplicationUserDoctor.PhoneNumber.Contains(searchDto.Criteria)
							|| d.ApplicationUserDoctor.Email.Contains(searchDto.Criteria)
							|| d.Specialization.Name.Contains(searchDto.Criteria));

			var doctors = await _unitOfWork.ManageDoctorRepo.GetAllAsync(searchDto.Page, searchDto.PageSize, criteria);


			var mappedDoctors = doctors.Select(d => new DoctorToReturnDto
			{
				PictureUrl = d.ApplicationUserDoctor.PictureUrl,
				FullName = d.ApplicationUserDoctor.FullName,
				Email = d.ApplicationUserDoctor.Email,
				PhoneNumber = d.ApplicationUserDoctor.PhoneNumber,
				Gender = d.ApplicationUserDoctor.Gender,
				Price = d.Price,
				DateOfBirth = d.ApplicationUserDoctor.DateOfBirth,
				Specialization = d.Specialization.Name
			}).ToList();

			return mappedDoctors;

		}


		public async Task<object> GetDoctorById(int id)
		{

			var doctor = await _unitOfWork.ManageDoctorRepo.GetByIdAsync(id);

			if (doctor is null)
				return "Invalid doctor Id";

			List<DoctorToReturnDto> doc = new List<DoctorToReturnDto>()
			{
				new DoctorToReturnDto
				{
				PictureUrl = doctor.ApplicationUserDoctor.PictureUrl,
				FullName = doctor.ApplicationUserDoctor.FullName,
				Email = doctor.ApplicationUserDoctor.Email,
				PhoneNumber = doctor.ApplicationUserDoctor.PhoneNumber,
				Specialization = doctor.Specialization.Name,
				Gender = doctor.ApplicationUserDoctor.Gender,
				DateOfBirth = doctor.ApplicationUserDoctor.DateOfBirth
				}

				};

			return doc;


		}
	};




}

















//return new
//{
//	pictureUrl = doctor.ApplicationUserDoctor.PictureUrl,
//	fullName = doctor.ApplicationUserDoctor.FullName,
//	email = doctor.ApplicationUserDoctor.Email,
//	phone = doctor.ApplicationUserDoctor.PhoneNumber,
//	specialize = doctor.Specialization.Name,
//	gender = doctor.ApplicationUserDoctor.Gender,
//	dateOfBirth = doctor.ApplicationUserDoctor.DateOfBirth
//};





