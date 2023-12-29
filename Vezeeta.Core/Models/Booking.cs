using Vezeeta.Core.Models.Identity;
using Vezeeta.Core.Utilities;

namespace Vezeeta.Core.Models
{
	public class Booking : BaseModel
	{
		public int DoctorId { get; set; }

		public Doctor Doctor { get; set; }

		public string PatientId { get; set; }

		public ApplicationUser Patient { get; set; }

		public int AppointmentTimeId { get; set; }

		public AppointmentTime AppointmentTime { get; set; }

		public int SpecializationId { get; set; }

		public Specialization Specialization { get; set; }

		public int? DiscountCodeId { get; set; }

		public DiscountCode? DiscountCode { get; set; }

		public int Price { get; set; }

		public decimal FinalPrice { get; set; }

		public BookingStatus BookingStatus { get; set; }

		public DateOnly CreatedAt { get; set; }





	}
}
