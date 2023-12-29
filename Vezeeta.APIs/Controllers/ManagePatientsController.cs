using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vezeeta.APIs.Errors;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Models.Identity;
using Vezeeta.Core.Services;

namespace Vezeeta.APIs.Controllers
{
	[Authorize(Roles = "Admin")]
	public class ManagePatientsController : BaseApiController
	{
		private readonly IManagePatientService _managePatientService;

		public ManagePatientsController(IManagePatientService managePatientService)
		{
			_managePatientService = managePatientService;
		}

		[ProducesResponseType(typeof(IReadOnlyList<ApplicationUser>), StatusCodes.Status200OK)]
		[HttpGet("getAllPatients")]
		public async Task<ActionResult<IReadOnlyList<ApplicationUser>>> GetAllPatients([FromQuery] SearchDto searchDto)

			=> Ok(await _managePatientService.GetAllPatients(searchDto));



		[ProducesResponseType(typeof(IReadOnlyList<ApplicationUser>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
		[HttpGet("getPatientById/{id}")]
		public async Task<ActionResult> GetPatientPatientById(string id)
		{
			var result = await _managePatientService.GetPatientById(id);

			if (result is null)
				return BadRequest(new ApiValidationErrorResponse
				{
					Errors = new string[] { "Invalid id" }
				});

			return Ok(result);
		}

	}
}
