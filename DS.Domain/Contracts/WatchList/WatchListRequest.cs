using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Domain.Contracts.WatchList
{
	public class WatchListRequest
	{
		public string EbayUser { get; set; }

		public DateTime? StartDate { get; set; }
	}
}
