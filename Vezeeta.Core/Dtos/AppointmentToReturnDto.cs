using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Vezeeta.Core.Dtos
{
	public class AppointmentToReturnDto
	{
		[Required]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public DayOfWeek DayOfWeek { get; set; }

		List<AppointmentTimeDto> Times { get; set; } = new List<AppointmentTimeDto>();
	}
}
