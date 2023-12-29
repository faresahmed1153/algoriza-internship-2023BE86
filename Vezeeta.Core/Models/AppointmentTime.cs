namespace Vezeeta.Core.Models
{
	public class AppointmentTime : BaseModel
	{

		public TimeOnly Time { get; set; }

		public bool IsBooked { get; set; }

		public int AppointmentId { get; set; }

		public  Appointment Appointment { get; set; }
	}
}