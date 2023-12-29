using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Vezeeta.Core.Models;
using Vezeeta.Core.Models.Identity;

namespace Vezeeta.Repository
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		}

		public DbSet<DiscountCode> DiscountCodes { get; set; }
		public DbSet<Specialization> Specializations { get; set; }
		public DbSet<Doctor> Doctors { get; set; }
		public DbSet<Appointment> Appointments { get; set; }
		public DbSet<AppointmentTime> AppointmentTimes { get; set; }
		public DbSet<Booking> Bookings { get; set; }

	}
}
