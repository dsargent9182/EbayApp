using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EbayApp.Models
{
	public class Student
	{

		public Guid? Id { get; set; }

		[Column("FirstName",TypeName = "varchar(50)")]
		[Display(Name = "First Name")]
		public string FirstMidName { get; set; }

		[Column(TypeName = "varchar(50)")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		[Column(TypeName = "datetime")]
		[DataType(DataType.Date)]
		[Display(Name = "Enrollment Date")]
		[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime EnrollmentDate { get; set; }

		[Column(TypeName = "datetime")]
		[Display(Name = "Created")]
		[ReadOnly(true)]
		public DateTime? Created { get; set; }

		public ICollection<Enrollment>? Enrollments { get; set; }
	}
}
