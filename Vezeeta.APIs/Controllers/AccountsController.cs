using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vezeeta.APIs.Errors;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Models.Identity;
using Vezeeta.Core.Services;
using Vezeeta.Core.Utilities;
using Vezeeta.Service.Helpers;

namespace Vezeeta.APIs.Controllers
{

	public class AccountsController : BaseApiController
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly ITokenService _tokenService;
		private readonly IMapper _mapper;

		public AccountsController(UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			RoleManager<IdentityRole> roleManager,
			ITokenService tokenService,
			IMapper mapper

			)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_tokenService = tokenService;
			_mapper = mapper;
		}

		[ProducesResponseType(typeof(UserToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
		[HttpPost("login")]
		public async Task<ActionResult<UserToReturnDto>> Login(LoginDto loginDto)
		{
			var user = await _userManager.FindByEmailAsync(loginDto.Email);

			if (user == null) return Unauthorized(new ApiResponse(401));

			var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);

			if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

			return Ok(new UserToReturnDto()
			{
				FullName = user.FullName,

				Email = loginDto.Email,

				Token = await _tokenService.CreateTokenAsync(user, _userManager)
			});
		}

		[ProducesResponseType(typeof(UserToReturnDto), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
		[HttpPost("register")]
		public async Task<ActionResult<UserToReturnDto>> Register([FromForm] PatientDto patientRegisterDto)
		{

			if (CheckEmailExists(patientRegisterDto.Email).Result.Value)

				return BadRequest(new ApiValidationErrorResponse()
				{
					Errors = new string[] { "email is already in use!" }
				});

			if (CheckPhoneExists(patientRegisterDto.PhoneNumber).Result.Value)

				return BadRequest(new ApiValidationErrorResponse()
				{
					Errors = new string[] { "phoneNumber is already in use!" }
				});


			if (patientRegisterDto.Picture is not null)

				patientRegisterDto.PictureUrl = await DocumentSettings.UploadFile(patientRegisterDto.Picture, "PatientImages");

			var mappedPatient = _mapper.Map<PatientDto, ApplicationUser>(patientRegisterDto);

			mappedPatient.UserName = patientRegisterDto.Email.Split("@")[0];

			mappedPatient.Discriminator = Role.Patient;

			var creatingUser = await _userManager.CreateAsync(mappedPatient, patientRegisterDto.Password);

			if (!creatingUser.Succeeded) return BadRequest(new ApiResponse(400));

			var patient = await _userManager.FindByEmailAsync(patientRegisterDto.Email);

			var patientRole = await _roleManager.FindByNameAsync(Role.Patient);

			var addingRoleResult = await _userManager.AddToRoleAsync(patient, patientRole.Name);

			if (!addingRoleResult.Succeeded) return BadRequest(new ApiResponse(400));

			var user = new UserToReturnDto()
			{
				FullName = string.Concat(patientRegisterDto.FirstName, " ", patientRegisterDto.LastName),

				Email = patientRegisterDto.Email,

				Token = await _tokenService.CreateTokenAsync(patient, _userManager)
			};

			return CreatedAtAction(nameof(Register), user);
		}

		[Authorize]
		[HttpGet]
		[ApiExplorerSettings(IgnoreApi = true)]
		public async Task<ActionResult<UserToReturnDto>> GetCurrentUser()
		{
			var email = User.FindFirstValue(ClaimTypes.Email);

			var user = await _userManager.FindByEmailAsync(email);

			return Ok(new UserToReturnDto()
			{
				FullName = user.UserName,
				Email = user.Email,
				Token = await _tokenService.CreateTokenAsync(user, _userManager)
			});
		}


		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet("emailExists")]
		public async Task<ActionResult<bool>> CheckEmailExists(string email)
		{
			return await _userManager.FindByEmailAsync(email) is not null;
		}


		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet("phoneExists")]
		public async Task<ActionResult<bool>> CheckPhoneExists(string phoneNumber)
		{

			return await _userManager.Users.AnyAsync(u => u.PhoneNumber == phoneNumber);
		}



	}
}
