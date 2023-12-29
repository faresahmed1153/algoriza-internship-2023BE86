using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vezeeta.APIs.Errors;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Services;

namespace Vezeeta.APIs.Controllers
{
	[Authorize(Roles = "Admin")]
	public class ManageDoctorsController : BaseApiController
	{
		private readonly IManageDoctorService _manageDoctorService;

		public ManageDoctorsController(IManageDoctorService manageDoctorService)
		{
			_manageDoctorService = manageDoctorService;
		}


		[ProducesResponseType(typeof(IReadOnlyList<DoctorToReturnDto>), StatusCodes.Status200OK)]
		[HttpGet("getAllDoctors")]
		public async Task<ActionResult<IReadOnlyList<DoctorToReturnDto>>> GetAllDoctors([FromQuery] SearchDto? searchDto)

				=> Ok(await _manageDoctorService.GetAllDoctors(searchDto));



		[ProducesResponseType(typeof(IReadOnlyList<DoctorToReturnDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
		[HttpGet("getDoctor/{id}")]
		public async Task<ActionResult<IReadOnlyList<DoctorToReturnDto>>> GetDoctorById(int id)

		{
			var doctor = await _manageDoctorService.GetDoctorById(id);
			if (doctor is string)
				return BadRequest(new ApiValidationErrorResponse
				{
					Errors = new string[] { "Invalid Id!" }
				});

			return Ok(doctor);
		}


		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
		[HttpPost("addDoctor")]
		public async Task<ActionResult> AddDoctor([FromForm] DoctorDto doctorDto)
		{

			var result = await _manageDoctorService.AddDoctor(doctorDto);
			if (result.Length > 0)
				return BadRequest(new ApiValidationErrorResponse
				{
					Errors = new string[] { $"{result}" }
				});

			return CreatedAtAction(nameof(AddDoctor), new { Message = "Doctor created successfully" });
		}




		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
		[HttpPut("updateDoctor")]
		public async Task<ActionResult<IReadOnlyList<DoctorToReturnDto>>> UpdateDoctor([FromForm] DoctorUpdateDto doctorUpdateDto)
		{
			var result = await _manageDoctorService.EditDoctor(doctorUpdateDto);
			if (result.Length > 0)
				return BadRequest(new ApiValidationErrorResponse
				{
					Errors = new string[] { $"{result}" }
				});

			return Ok(new { Message = "Doctor updated successfully" });
		}

		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
		[HttpDelete("deleteDoctor/{id}")]
		public async Task<ActionResult<IReadOnlyList<DoctorToReturnDto>>> DeleteDoctor(int id)
		{
			var result = await _manageDoctorService.DeleteDoctor(id);
			if (result.Length > 0)
				return BadRequest(new ApiValidationErrorResponse
				{
					Errors = new string[] { $"{result}" }
				});

			return Ok(new { Message = "Doctor deleted successfully" });
		}

	}
}
