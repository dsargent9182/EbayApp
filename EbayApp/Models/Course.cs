using System.ComponentModel.DataAnnotations.Schema;

namespace EbayApp.Models
{
	public class Course
	{
		public Guid CourseId { get; set; }

		public string Title { get; set; }

		public int Credits { get; set; }

		[Column(TypeName ="datetime")]
		public DateTime? Created { get; set; }

		public ICollection<Enrollment>? Enrollments { get; set; }

	}
}
