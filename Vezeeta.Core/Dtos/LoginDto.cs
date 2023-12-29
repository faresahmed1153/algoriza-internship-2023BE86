using System.ComponentModel.DataAnnotations;

namespace Vezeeta.Core.Dtos
{
	public class LoginDto
	{

		[EmailAddress]
		[RegularExpression(@"^\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z$",
			ErrorMessage = "Invalid Email Format")]
		public string Email { get; set; }


		[RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
			ErrorMessage = "password Has minimum 8 characters in length and include at least 1 lowercase, at least 1 uppercase, at least 1 numeric character and at least one special character ")]
		public string Password { get; set; }
	}
}
