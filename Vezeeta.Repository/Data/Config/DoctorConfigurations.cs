using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vezeeta.Core.Models;

namespace Vezeeta.Repository.Data.Config
{
	public class DoctorConfigurations : IEntityTypeConfiguration<Doctor>
	{
		public void Configure(EntityTypeBuilder<Doctor> builder)
		{

			// so that the admin can create a doctor without assigning a price to the doctor
			// then the doctor can set his own price when adding his appointments
			builder.Property(d => d.Price)
				.HasDefaultValue(0)
				.HasColumnType("decimal(18,2)");

			builder.Property(d => d.CreatedAt)
				.HasDefaultValueSql("getdate()");


			builder.HasOne(d => d.Specialization)
				.WithMany(d => d.Doctors)
				.HasForeignKey(d => d.SpecializationId)
				.OnDelete(DeleteBehavior.Restrict);


			builder.HasOne(d => d.ApplicationUserDoctor)
				.WithOne(d => d.Doctor)
				.HasForeignKey<Doctor>(d => d.ApplicationUserId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
