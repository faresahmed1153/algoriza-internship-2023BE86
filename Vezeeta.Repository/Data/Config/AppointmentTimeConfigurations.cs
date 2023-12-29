using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vezeeta.Core.Models;

namespace Vezeeta.Repository.Data.Config
{
	public class AppointmentTimeConfigurations : IEntityTypeConfiguration<AppointmentTime>
	{
		public void Configure(EntityTypeBuilder<AppointmentTime> builder)
		{
			builder.Property(a => a.IsBooked).HasDefaultValue(false);

			builder.HasOne(a => a.Appointment)
				.WithMany(a => a.AppointmentTimes)
				.HasForeignKey(a => a.AppointmentId)
				.OnDelete(DeleteBehavior.Cascade);

		}
	}
}
