using Microsoft.AspNetCore.Identity;
using Vezeeta.Core.Models.Identity;
using Vezeeta.Core.Utilities;

namespace Vezeeta.Repository
{
	//please note that i am calling this static method inside a try-catch outside 
	// please also note that i have added a discriminator colmun in AppUser table just to
	// be able to filter users as patients in the database not in the application because if 
	// i used usermanager and called GetUsersInRoleAsync(Role.Patient) and made a pagination and 
	// search on it the filteration will happen in the application because where will return IEnumerable 
	public class AdminSeed
	{
		public static async Task SeedAsync(

			UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager)
		{
			if (!userManager.Users.Any())
			{
				var user = new ApplicationUser()
				{
					FullName = "Fares Ahmed",
					Email = "faresahmed@gmail.com",
					UserName = "faresahmed",
					PhoneNumber = "01012264789",
					Gender = Gender.Male,
					DateOfBirth = new DateOnly(1990, 11, 14),
					Discriminator = Role.Admin
				};

				await userManager.CreateAsync(user, "Admin@942");


				var admin = await userManager.FindByEmailAsync("faresahmed@gmail.com");

				var adminRole = await roleManager.FindByNameAsync(Role.Admin);

				await userManager.AddToRoleAsync(admin, adminRole.Name);

			};


		}

	}
}

