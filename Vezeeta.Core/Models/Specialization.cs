namespace Vezeeta.Core.Models
{
    public class Specialization : BaseModel
	{
		public string Name { get; set; }

		public  ICollection<Doctor> Doctors { get; set; } = new HashSet<Doctor>();

		public  ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
	}
}
