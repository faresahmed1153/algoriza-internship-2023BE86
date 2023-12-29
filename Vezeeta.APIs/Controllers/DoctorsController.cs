using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vezeeta.APIs.Errors;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Services;

namespace Vezeeta.APIs.Controllers
{
	[Authorize(Roles = "Doctor")]
	public class DoctorsController : BaseApiController
	{
		private readonly IDoctorService _doctorService;

		public DoctorsController(IDoctorService doctorService)
		{
			_doctorService = doctorService;
		}

		[HttpPost("addAppointments")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> AddAppointments(AppointmentsDto appointmentsDto)
		{


			var errors = ValdidateAppointments(appointmentsDto);

			if (errors.Length > 0)
				return BadRequest(new ApiValidationErrorResponse
				{
					Errors = new string[] { errors }
				});


			var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var result = await _doctorService.AddDoctorAppointments(appointmentsDto, appUserId);


			if (result.Length == 0)
				return CreatedAtAction(nameof(AddAppointments), new { Message = "Appointments added successfully" });


			return BadRequest(new ApiValidationErrorResponse
			{
				Errors = new string[] { result }
			});

		}

		[HttpPatch("confrimCheckUp/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> ConfrimCheckUp(int id)
		{
			var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var result = await _doctorService.UpdateBookingStatus(id, appUserId);

			if (result.Length > 0)
				return BadRequest(new ApiValidationErrorResponse
				{
					Errors = new string[] { result }
				});

			return Ok(new { Message = "CheckUp confirmed Successfully!" });
		}

		[HttpGet("getAllBooking")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IReadOnlyList<object>>> GetAllBookings([FromQuery] SearchDto searchDto)
		{
			var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			return Ok(await _doctorService.GetAllBookings(searchDto, appUserId));
		}


		[HttpPatch("updateTime")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult> UpdateTime(TimeDto timeDto)
		{
			var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var errors = await _doctorService.UpdateAppointmentTime(timeDto, appUserId);

			if (errors.Length > 0)
				return BadRequest(new ApiValidationErrorResponse
				{
					Errors = new string[] { errors }
				});

			return Ok(new { Message = "Time updated Successfully!" });
		}


		[HttpDelete("deleteTime/{timeId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult> DeleteTime(int timeId)
		{
			var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var errors = await _doctorService.DeleteAppointmentTime(timeId, appUserId);

			if (errors.Length > 0)
				return BadRequest(new ApiValidationErrorResponse
				{
					Errors = new string[] { errors }
				});

			return Ok(new { Message = "Time deleted Successfully!" });
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		private string ValdidateAppointments(AppointmentsDto appointmentsDto)
		{
			HashSet<string> days = new HashSet<string>();
			HashSet<string> times = new HashSet<string>();

			foreach (var day in appointmentsDto.Days)
			{
				if (!days.Contains(day.DayOfWeek.ToString()))
				{
					days.Add(day.DayOfWeek.ToString());

					foreach (var time in day.Times)
					{
						if (!times.Contains(time.ToString()))
						{
							times.Add(time.ToString());

						}

						else
							return "you can't have a repeated values on times in the same day!";

					}

				}

				else
				{
					return "you can't have a repeated value on days";
				}
			}
			return "";

		}
	}
}

