using Vezeeta.Core.Utilities;

namespace Vezeeta.Core.Models
{
	public class DiscountCode : BaseModel
	{
		public string Code { get; set; }

		public decimal Value { get; set; }

		public DiscountType Type { get; set; }

		public int BookingsCompleted { get; set; }

		public bool IsActive { get; set; }

		public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();

	}
}
