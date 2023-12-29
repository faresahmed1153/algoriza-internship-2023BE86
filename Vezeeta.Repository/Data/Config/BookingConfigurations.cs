using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vezeeta.Core.Models;

namespace Vezeeta.Repository.Data.Config
{
	public class BookingConfigurations : IEntityTypeConfiguration<Booking>
	{
		public void Configure(EntityTypeBuilder<Booking> builder)
		{
			builder.HasOne(b => b.Doctor).WithMany(b => b.Bookings)
				.HasForeignKey(b => b.DoctorId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(b => b.Patient).WithMany(b => b.Bookings)
				.HasForeignKey(b => b.PatientId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(b => b.AppointmentTime).WithMany()
				.HasForeignKey(b => b.AppointmentTimeId)
			.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(b => b.Specialization).WithMany(b => b.Bookings)
				.HasForeignKey(b => b.SpecializationId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(b => b.DiscountCode).WithMany(b => b.Bookings)
				.HasForeignKey(b => b.DiscountCodeId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Property(b => b.FinalPrice)
				.HasColumnType("decimal(18,2)");

			builder.Property(b => b.CreatedAt)
				.HasDefaultValueSql("getdate()");

		}
	}
}
