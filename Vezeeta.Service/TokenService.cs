using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Vezeeta.Core.Models.Identity;
using Vezeeta.Core.Services;

namespace Vezeeta.Service
{
	public class TokenService : ITokenService
	{
		private readonly IConfiguration _configuration;

		public TokenService(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public async Task<string> CreateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager)
		{
			// private claims
			var authClaims = new List<Claim>()
			{
				new Claim(ClaimTypes.GivenName,user.UserName),

				new Claim(ClaimTypes.Email, user.Email),

				new Claim(ClaimTypes.NameIdentifier,user.Id)

			};


			var userRoles = await userManager.GetRolesAsync(user);

			foreach (var role in userRoles)
				authClaims.Add(new Claim(ClaimTypes.Role, role));


			//secret key
			var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

			var token = new JwtSecurityToken(
				issuer: _configuration["Jwt:ValidIssure"],
				audience: _configuration["Jwt:ValidAudience"],
				expires: DateTime.Now.AddDays(double.Parse(_configuration["Jwt:DurationInDays"])),
				claims: authClaims,
				signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)

				);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
