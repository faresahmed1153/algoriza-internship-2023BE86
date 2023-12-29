using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vezeeta.APIs.Errors;
using Vezeeta.Core.Services;

namespace Vezeeta.APIs.Controllers
{
	[Authorize(Roles = "Admin")]
	public class DashBoardsController : BaseApiController
	{
		private readonly IDashBoardService _dashBoardService;


		public DashBoardsController(IDashBoardService dashBoardService)
		{
			_dashBoardService = dashBoardService;
		}

		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
		[HttpGet("numOfDoctors")]
		public async Task<ActionResult<int>> GetNumOfDoctors([FromQuery] DateOnly? date)
		{
			if (date.HasValue)
			{
				var isValid = ValidateDateTimeOfSearch(date);

				if (!isValid)
					return BadRequest(new ApiValidationErrorResponse
					{
						Errors = new string[] { "Date must be yesterday or last week or last month or last year" }
					});

				int numberOfDoctors = await _dashBoardService.NumOfDoctors(date);

				return Ok(new { NumberOfDoctors = numberOfDoctors });
			}
			int numOfDoctors = await _dashBoardService.NumOfDoctors();

			return Ok(new { NumberOfDoctors = numOfDoctors });
		}




		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
		[HttpGet("numOfPatients")]
		public async Task<ActionResult<int>> GetNumOfPatients([FromQuery] DateOnly? date)

		{
			if (date.HasValue)
			{
				var isValid = ValidateDateTimeOfSearch(date);

				if (!isValid)
					return BadRequest(new ApiValidationErrorResponse
					{
						Errors = new string[] { "Date must be yesterday or last week or last month or last year" }
					});

				int numOfPatients = await _dashBoardService.NumOfPatients(date);

				return Ok(new { NumberOfPatients = numOfPatients });
			}

			int numberOfPatients = await _dashBoardService.NumOfPatients();
			return Ok(new { NumberOfPatients = numberOfPatients });
		}




		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
		[HttpGet("numOfBookings")]
		public async Task<ActionResult<object>> GetNumOfBookings([FromQuery] DateOnly? date)
		{
			if (date.HasValue)
			{
				var isValid = ValidateDateTimeOfSearch(date);

				if (!isValid)
					return BadRequest(new ApiValidationErrorResponse
					{
						Errors = new string[] { "Date must be yesterday or last week or last month or last year" }
					});

				return Ok(await _dashBoardService.NumOfBookings(date));
			}

			return Ok(await _dashBoardService.NumOfBookings());
		}


		[ProducesResponseType(StatusCodes.Status200OK)]
		[HttpGet("topFiveSpecializations")]
		public async Task<ActionResult<IReadOnlyList<object>>> TopFiveSpecializations()

			=> Ok(await _dashBoardService.TopFiveSpecializations(5));



		[ProducesResponseType(StatusCodes.Status200OK)]
		[HttpGet("topTenDoctors")]
		public async Task<ActionResult<IReadOnlyList<object>>> TopTenDoctors()

			=> Ok(await _dashBoardService.TopTenDoctors(10));





		[ApiExplorerSettings(IgnoreApi = true)]
		private bool ValidateDateTimeOfSearch(DateOnly? dateTime)
		{

			DateOnly now = DateOnly.FromDateTime(DateTime.Now);

			var yesterday = now.AddDays(-1);

			var lastWeek = now.AddDays(-7);

			var lastMonth = now.AddMonths(-1);

			var lastYear = now.AddYears(-1);

			if (dateTime == yesterday || dateTime == lastWeek || dateTime == lastMonth | dateTime == lastYear)

				return true;

			return false;

		}
	}
}
