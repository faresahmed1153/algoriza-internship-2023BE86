using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Vezeeta.Core.Models;


namespace Vezeeta.Repository.Data
{
	public static class AppDbContextSeed
	{
		public static async Task SeedAsync(AppDbContext dbContext)
		{

			if (!await dbContext.Roles.AnyAsync())
			{
				var rolesData = File.ReadAllText("../Vezeeta.Repository/Data/DataSeed/roles.json");
				var roles = JsonSerializer.Deserialize<List<IdentityRole>>(rolesData);
				if (roles?.Count() > 0)
				{
					await dbContext.Set<IdentityRole>().AddRangeAsync(roles);
					await dbContext.SaveChangesAsync();
				}

			}

			if (!await dbContext.Specializations.AnyAsync())
			{
				var specializationsData = File.ReadAllText("../Vezeeta.Repository/Data/DataSeed/specializations.json");
				var specializations = JsonSerializer.Deserialize<List<Specialization>>(specializationsData);
				if (specializations?.Count() > 0)
				{

					await dbContext.Set<Specialization>().AddRangeAsync(specializations);
					await dbContext.SaveChangesAsync();
				}

			}

			if (!await dbContext.DiscountCodes.AnyAsync())
			{
				var discountcodesData = File.ReadAllText("../Vezeeta.Repository/Data/DataSeed/discountCodes.json");
				var discountCodes = JsonSerializer.Deserialize<List<DiscountCode>>(discountcodesData);
				if (discountCodes?.Count() > 0)
				{
					await dbContext.Set<DiscountCode>().AddRangeAsync(discountCodes);
					await dbContext.SaveChangesAsync();
				}

			}



		}
	}
}
