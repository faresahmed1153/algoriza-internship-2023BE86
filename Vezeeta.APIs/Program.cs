using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Vezeeta.APIs.Extentions;
using Vezeeta.APIs.Middlewares;
using Vezeeta.Core.Models.Identity;
using Vezeeta.Repository;
using Vezeeta.Repository.Data;

namespace Vezeeta.APIs
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			#region Configure Services

			builder.Services.AddControllers();


			builder.Services.AddSwaggerServices();

			builder.Services.AddDbContext<AppDbContext>(options =>

			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});

			builder.Services.AddApplicationServices(builder.Configuration);

			builder.Services.AddIdentityServices(builder.Configuration);

			builder.Services.AddCors(options =>
			options.AddPolicy("MyPolicy", options =>
			options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin())
			);

			#endregion



			var app = builder.Build();


			//Data Seeding

			using var scope = app.Services.CreateScope();

			var services = scope.ServiceProvider;
			var loggerFactory = services.GetRequiredService<ILoggerFactory>();
			try
			{

				var dbContext = services.GetRequiredService<AppDbContext>();

				await dbContext.Database.MigrateAsync();

				await AppDbContextSeed.SeedAsync(dbContext);

				var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

				var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

				await AdminSeed.SeedAsync(userManager, roleManager);
			}
			catch (Exception ex)
			{

				var logger = loggerFactory.CreateLogger<Program>();

				logger.LogError(ex, "an error occured during apply the migrations");
			}



			// Configure the HTTP request pipeline.

			#region Configure Kestrel Middlewares

			app.UseMiddleware<ExceptionMiddleware>();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwaggerMiddleWares();
			}

			app.UseStatusCodePagesWithReExecute("/errors/{0}");

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseCors("MyPolicy");

			app.UseAuthentication();

			app.UseAuthorization();

			app.MapControllers();

			#endregion


			app.Run();
		}
	}
}
