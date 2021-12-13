namespace EbayApp.Models
{
	public class Course
	{
		public Guid CourseId { get; set; }

		public string Title { get; set; }

		public int Credits { get; set; }

		public ICollection<Enrollment> Enrollments { get; set; }

	}
}
