using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vezeeta.Core.Models;

namespace Vezeeta.Repository.Data.Config
{
	public class DiscountCodeConfigurations : IEntityTypeConfiguration<DiscountCode>
	{
		public void Configure(EntityTypeBuilder<DiscountCode> builder)
		{


			builder.HasIndex(d => d.Code)
				.IsUnique();

			builder.Property(d => d.IsActive)
				.HasDefaultValue(true);

			builder.Property(d => d.Value)
				.HasColumnType("decimal(18,2)");

		}
	}
}
