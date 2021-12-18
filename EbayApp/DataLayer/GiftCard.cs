using System.ComponentModel.DataAnnotations;

namespace EbayApp.DataLayer
{
	public class GiftCard
	{
		public int Id { get; set; }

		public string Store { get; set; }

		public string Number { get; set; }

		public string PIN { get; set; }

		[DisplayFormat(DataFormatString = "{0:C2}")]
		public decimal Amount { get; set; }

		[DisplayFormat(DataFormatString = "{0:C2}")]
		public decimal Balance { get; set; }

		[Display( Name = "Created")]
		[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime CreatedDate { get; set; }

		[Display( Name = "Last Update Date")]
		[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime? DateM { get; set; }
	}
}
