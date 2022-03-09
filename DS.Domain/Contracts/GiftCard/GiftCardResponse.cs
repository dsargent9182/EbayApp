using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Domain.Contracts.GiftCard
{
	public class GiftCardResponse
	{
		public IEnumerable<DS.Domain.Models.GiftCard> GiftCards { get; set; }
	}
}
