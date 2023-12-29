using System.ComponentModel.DataAnnotations;

namespace Vezeeta.Core.Dtos
{
	public class AppointmentsDto
	{
		[Range(100, int.MaxValue, ErrorMessage = "Minimum value is 100")]
		public int Price { get; set; }


		public List<AppointmentDto> Days { get; set; }

	}
}
