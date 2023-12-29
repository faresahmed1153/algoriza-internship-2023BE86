using Microsoft.AspNetCore.Mvc;
using Vezeeta.APIs.Errors;
using Vezeeta.Core;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Services;
using Vezeeta.Repository;
using Vezeeta.Service;
using Vezeeta.Service.Helpers;

namespace Vezeeta.APIs.Extentions
{
	public static class ApplicationServicesExtention
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{

			services.AddScoped<IUnitOfWork, UnitOfWork>();

			services.AddScoped<IDashBoardService, DashBoardService>();

			services.AddScoped<IDiscountCodeService, DiscountCodeService>();

			services.AddScoped<IManageDoctorService, ManageDoctorService>();

			services.AddScoped<IManagePatientService, ManagePatientService>();

			services.AddScoped<IDoctorService, DoctorService>();

			services.AddScoped<IPatientService, PatientService>();

			services.AddAutoMapper(typeof(MappingProfiles));

			services.Configure<EmailDetailsDto>(configuration.GetSection("MailSettings"));

			services.AddTransient<IEmailService, EmailService>();

			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = (actionContext) =>
				{
					var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
					.SelectMany(P => P.Value.Errors)
					.Select(E => E.ErrorMessage)
					.ToArray();
					var validationErrorResponse = new ApiValidationErrorResponse()
					{
						Errors = errors
					};
					return new BadRequestObjectResult(validationErrorResponse);
				};
			}
		   );
			return services;
		}
	}
}
