using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vezeeta.APIs.Errors;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Services;

namespace Vezeeta.APIs.Controllers
{
	[Authorize(Roles = "Patient")]
	public class PatientsController : BaseApiController
	{
		private readonly IPatientService _patientService;

		public PatientsController(IPatientService patientService)
		{
			_patientService = patientService;
		}

		[HttpPatch("cancelBooking/{bookingId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> CancelBooking(int bookingId)
		{
			var patientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var errors = await _patientService.CancelBooking(bookingId, patientId);

			if (errors.Length > 0)
				return BadRequest(new ApiValidationErrorResponse
				{
					Errors = new string[] { errors }
				});

			return Ok(new { Message = "Booking cancelled successfully" });

		}

		[HttpGet("getAllDoctors")]
		[ProducesResponseType(typeof(IReadOnlyList<object>), StatusCodes.Status200OK)]
		public async Task<ActionResult<IReadOnlyList<object>>> GetAllDoctors([FromQuery] SearchDto searchDto)

		=> Ok(await _patientService.GetAllDoctors(searchDto));



		[HttpGet("getAllBookings")]
		[ProducesResponseType(typeof(IReadOnlyList<object>), StatusCodes.Status200OK)]
		public async Task<ActionResult<IReadOnlyList<object>>> getAllBookings()
		{
			var patientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			return Ok(await _patientService.GetAllBookings(patientId));
		}



		[HttpPost("makeBooking")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> MakeBooking(BookingDto bookingDto)
		{
			var patientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var errors = await _patientService.BookAppointment(bookingDto, patientId);

			if (errors.Length > 0)
				return BadRequest(new ApiValidationErrorResponse
				{
					Errors = new string[] { errors }
				});

			return CreatedAtAction(nameof(MakeBooking), (new { Message = "Booking Created successfully" })); ;
		}

	}
}
