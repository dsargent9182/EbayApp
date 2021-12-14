using EbayApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EbayApp.Data
{
	public class CourseEntityTypeConfiguration : IEntityTypeConfiguration<Course>
	{
		public void Configure(EntityTypeBuilder<Course> builder)
		{
			builder.ToTable("Course");
			builder.Property(e => e.CourseId)
				.HasDefaultValueSql("NEWSEQUENTIALID()");
			builder.Property(e => e.Created)
			.HasDefaultValueSql("getdate()")
			.IsRequired()
			.Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);
			
		}
	}
}
