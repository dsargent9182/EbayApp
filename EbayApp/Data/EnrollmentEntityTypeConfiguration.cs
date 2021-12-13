using EbayApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EbayApp.Data
{
	public class EnrollmentEntityTypeConfiguration : IEntityTypeConfiguration<Enrollment>
	{
		public void Configure(EntityTypeBuilder<Enrollment> builder)
		{
			builder.ToTable("Enrollment");
			builder.Property(e => e.EnrollmentId)
				.HasDefaultValueSql("NEWSEQUENTIALID()");
			
		}
	}
}
