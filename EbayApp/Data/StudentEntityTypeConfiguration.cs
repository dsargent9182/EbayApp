using EbayApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EbayApp.Data
{
	public class StudentEntityTypeConfiguration : IEntityTypeConfiguration<Student>
	{
		public void Configure(EntityTypeBuilder<Student> builder)
		{
			builder.ToTable("Student");
			builder.Property(e => e.Id)
				.HasDefaultValueSql("NEWSEQUENTIALID()");
			builder.Property(e => e.Created)
			.HasDefaultValueSql("getdate()")
			.IsRequired()
			.Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);
		}
	}
}
