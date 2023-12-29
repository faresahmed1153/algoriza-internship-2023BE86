using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vezeeta.Core.Models;

namespace Vezeeta.Repository.Data.Config
{
	public class SpecializationConfigurations : IEntityTypeConfiguration<Specialization>
	{
		public void Configure(EntityTypeBuilder<Specialization> builder)
		{

			builder.HasIndex(d => d.Name)
				.IsUnique();
		}
	}
}
