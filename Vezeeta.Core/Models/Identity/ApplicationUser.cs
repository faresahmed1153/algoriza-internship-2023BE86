using Microsoft.AspNetCore.Identity;
using Vezeeta.Core.Utilities;

namespace Vezeeta.Core.Models.Identity
{
	public class ApplicationUser : IdentityUser
	{
		public string FullName { get; set; }

		public string? PictureUrl { get; set; }

		public Gender Gender { get; set; }

		public DateOnly DateOfBirth { get; set; }

		public DateOnly CreatedAt { get; set; }

		public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();

		public Doctor? Doctor { get; set; }

		public string Discriminator { get; set; }
	}
}
