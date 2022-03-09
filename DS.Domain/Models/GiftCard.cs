using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Domain.Models
{
	public class GiftCard
	{
		public int Id { get; set; }

		public string Store { get; set; }

		public string Number { get; set; }

		public string PIN { get; set; }

		public decimal Amount { get; set; }

		public decimal Balance { get; set; }

		public DateTime CreatedDate { get; set; }

		public DateTime? DateM { get; set; }
	}
}
