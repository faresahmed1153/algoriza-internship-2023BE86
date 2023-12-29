namespace Vezeeta.Core.Models
{
	public class Appointment : BaseModel
	{
		public int DoctorId { get; set; }

		public Doctor Doctor { get; set; }

		public DayOfWeek DayOfWeek { get; set; }

		public ICollection<AppointmentTime> AppointmentTimes { get; set; } = new HashSet<AppointmentTime>();

	}


}
