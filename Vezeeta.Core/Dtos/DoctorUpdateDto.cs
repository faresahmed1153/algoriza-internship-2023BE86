using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Vezeeta.Core.Utilities;

namespace Vezeeta.Core.Dtos
{

	public class DoctorUpdateDto
	{
		[Required]
		public int Id { get; set; }

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
		[Phone]
		[RegularExpression("^01[0125][0-9]{8}$",
			ErrorMessage = "Phone number must be 11 numbers and starts with 010 or 011 or 012 or 015")]
		public string PhoneNumber { get; set; }


		[Required]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public Gender Gender { get; set; }


		[Required]
		public DateOnly DateOfBirth { get; set; }

		public IFormFile? Picture { get; set; }

		public string? PictureUrl { get; set; }

		public int SpecializationId { get; set; }
	}
}
