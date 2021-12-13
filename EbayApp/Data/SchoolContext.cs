using Microsoft.EntityFrameworkCore;
using EbayApp.Models;

namespace EbayApp.Data
{
	public class SchoolContext : DbContext
	{
		public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
		{

		}

		public DbSet<Course> Courses { get; set; }

		public DbSet<Enrollment> Enrollments { get; set; }

		public DbSet<Student> Students { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			new CourseEntityTypeConfiguration().Configure(modelBuilder.Entity<Course>());
			new StudentEntityTypeConfiguration().Configure(modelBuilder.Entity<Student>());
			new EnrollmentEntityTypeConfiguration().Configure(modelBuilder.Entity<Enrollment>());

			//modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
		}
	}
}
