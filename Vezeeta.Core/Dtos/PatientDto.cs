using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Vezeeta.Core.Utilities;

namespace Vezeeta.Core.Dtos
{


	public class PatientDto
	{

		public string FirstName { get; set; }


		public string LastName { get; set; }


		[EmailAddress]
		[RegularExpression(@"^\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z$",
			ErrorMessage = "Invalid Email Format")]
		public string Email { get; set; }


		[RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
			ErrorMessage = "password Has minimum 8 characters in length and include at least 1 lowercase, at least 1 uppercase, at least 1 numeric character and at least one special character ")]
		public string Password { get; set; }



		[Phone]
		[RegularExpression("^01[0125][0-9]{8}$",
			ErrorMessage = "Phone number must be 11 numbers and starts with 010 or 011 or 012 or 015")]
		public string PhoneNumber { get; set; }



		[JsonConverter(typeof(JsonStringEnumConverter))]
		public Gender Gender { get; set; }



		public DateOnly DateOfBirth { get; set; }


		public IFormFile? Picture { get; set; }

		public string? PictureUrl { get; set; }
	}
}
