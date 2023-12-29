using System.Text.Json.Serialization;
using Vezeeta.Core.Utilities;

namespace Vezeeta.Core.Dtos
{
	public class SearchDoctorsDto
	{
		public string PictureUrl { get; set; }

		public string FullName { get; set; }

		public string Email { get; set; }

		public string PhoneNumber { get; set; }

		public string Specialization { get; set; }

		public decimal Price { get; set; }

		public DateOnly DateOfBirth { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public Gender Gender { get; set; }

		List<AppointmentToReturnDto> Appointments { get; set; } = new List<AppointmentToReturnDto>();
	}
}
