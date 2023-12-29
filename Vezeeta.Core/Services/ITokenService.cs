using Microsoft.AspNetCore.Identity;
using Vezeeta.Core.Models.Identity;

namespace Vezeeta.Core.Services
{
	public interface ITokenService
	{
		Task<string> CreateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager);
	}
}
