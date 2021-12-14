using System.ComponentModel.DataAnnotations.Schema;

namespace EbayApp.Models
{
	public enum Grade
	{
		A, B, C, D, F
	}
	public class Enrollment
	{
		public Guid EnrollmentId { get; set; }

		public Guid CourseId { get; set; }

		public Guid StudentId { get; set; }

		public Grade? Grade { get; set; }

		public Course Course { get; set; }
		public Student Student { get; set; }

		[Column(TypeName="datetime")]
		public DateTime? Created { get; set; }

	}
}
