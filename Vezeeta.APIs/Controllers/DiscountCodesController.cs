using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vezeeta.APIs.Errors;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Models;
using Vezeeta.Core.Services;
using Vezeeta.Core.Utilities;

namespace Vezeeta.APIs.Controllers
{
	[Authorize(Roles = "Admin")]
	public class DiscountCodesController : BaseApiController
	{
		private readonly IDiscountCodeService _discountCodeService;
		private readonly IMapper _mapper;

		public DiscountCodesController(
			IDiscountCodeService discountCodeService, IMapper mapper)
		{
			_discountCodeService = discountCodeService;
			_mapper = mapper;
		}

		[HttpPost("addDiscountCode")]
		[ProducesResponseType(typeof(DiscountCode), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> AddDiscountCode(DiscountCodeDto discountCodeDto)
		{
			if (discountCodeDto.Type == DiscountType.Percentage && discountCodeDto.Value > 100)

				return BadRequest(new ApiValidationErrorResponse()
				{
					Errors = new string[] { "value of discountCode can't be greater than 100 with percentage Discount Type" }
				});

			discountCodeDto.Code.ToLower();

			var mappedDiscountCode = _mapper.Map<DiscountCodeDto, DiscountCode>(discountCodeDto);

			mappedDiscountCode.IsActive = true;

			var result = await _discountCodeService.AddDiscountCode(mappedDiscountCode);

			if (result.Length == 0)

				return CreatedAtAction(nameof(AddDiscountCode), mappedDiscountCode);

			return BadRequest(new ApiValidationErrorResponse()
			{
				Errors = new string[] { result }
			});
		}


		[HttpPut("updateDiscountCode")]
		[ProducesResponseType(typeof(DiscountCode), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> UpdateDiscountCode(DiscountCodeDto discountCodeDto)
		{
			if (discountCodeDto.Type == DiscountType.Percentage && discountCodeDto.Value > 100)

				return BadRequest(new ApiValidationErrorResponse()
				{
					Errors = new string[] { "value of discountCode can't be greater than 100 with percentage Discount Type" }
				});

			discountCodeDto.Code.ToLower();

			var mappedDiscountCode = _mapper.Map<DiscountCodeDto, DiscountCode>(discountCodeDto);

			var result = await _discountCodeService.UpdateDiscountCode(mappedDiscountCode);

			if (result.Length == 0)

				return Ok(mappedDiscountCode);

			return BadRequest(new ApiValidationErrorResponse()
			{
				Errors = new string[] { result }
			});
		}


		[HttpDelete("deleteDiscountCode/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> DeleteDiscountCode(int id)
		{

			var result = await _discountCodeService.DeleteDiscountCode(id);

			if (result.Length == 0)

				return Ok(new { message = $"Discount Code with Id {id} was deleted successfully" });

			return BadRequest(new ApiValidationErrorResponse()
			{
				Errors = new string[] { result }
			});
		}


		[HttpPatch("deactivateDiscountCode/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> deactivateDiscountCode(int id)
		{

			var result = await _discountCodeService.DeactivateDiscountCode(id);

			if (result.Length == 0)

				return Ok(new { message = $"Discount Code with Id {id} was Deactivated successfully" });

			return BadRequest(new ApiValidationErrorResponse()
			{
				Errors = new string[] { result }
			});
		}

	}
}
