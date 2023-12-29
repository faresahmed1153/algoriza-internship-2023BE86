using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Vezeeta.Core.Utilities;

namespace Vezeeta.Core.Dtos
{

	public class DoctorDto
	{

		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required]
		[EmailAddress]
		[RegularExpression(@"^\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z$",
			ErrorMessage = "Invalid Email Format")]
		public string Email { get; set; }

		[Required]
		[RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
			ErrorMessage = "password Has minimum 8 characters in length and include at least 1 lowercase, at least 1 uppercase, at least 1 numeric character and at least one special character ")]
		public string Password { get; set; }


		[Required]
		[Phone]
		[RegularExpression("^01[0125][0-9]{8}$",
			ErrorMessage = "Phone number must be 11 numbers and starts with 010 or 011 or 012 or 015")]
		public string PhoneNumber { get; set; }


		[Required]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public Gender Gender { get; set; }


		[Required]
		public DateOnly DateOfBirth { get; set; }

		public IFormFile Picture { get; set; }

		public string? PictureUrl { get; set; }

		public int SpecializationId { get; set; }
	}
}
