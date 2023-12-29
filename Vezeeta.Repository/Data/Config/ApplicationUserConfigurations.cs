using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vezeeta.Core.Models.Identity;

namespace Vezeeta.Repository.Data.Config
{
	public class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{


			builder.Property(u => u.CreatedAt)
				.HasDefaultValueSql("getdate()");

		}
	}
}
