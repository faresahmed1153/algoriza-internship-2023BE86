using Vezeeta.Core.Models;

namespace Vezeeta.Core.Dtos
{
	public class DoctorWithSpecificAppointmentToReturnDto
	{
		public Doctor Doctor { get; set; }

		public Appointment Appointment { get; set; }

		public AppointmentTime AppointmentTime { get; set; }

		public Specialization Specialization { get; set; }
	}
}
