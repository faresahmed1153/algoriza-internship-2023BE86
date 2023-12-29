using Vezeeta.Core.Models.Identity;

namespace Vezeeta.Core.Models
{
	public class Doctor : BaseModel
	{

		public int Price { get; set; }

		public string ApplicationUserId { get; set; }

		public ApplicationUser ApplicationUserDoctor { get; set; }

		public int SpecializationId { get; set; }

		public Specialization Specialization { get; set; }

		public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();

		public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();

		public DateOnly CreatedAt { get; set; }

	}
}
