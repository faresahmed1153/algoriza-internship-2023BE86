using System.Text.Json.Serialization;

namespace Vezeeta.Core.Dtos
{
	public class AppointmentDto
	{
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public DayOfWeek DayOfWeek { get; set; }


		public List<TimeOnly> Times { get; set; }
	}
}
