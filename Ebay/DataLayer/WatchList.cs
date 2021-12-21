using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.MicroService.DataLayer
{
	public class WatchList
	{		
		public Int64 EbayId { get; set; }

		public string Title { get; set; }

		public int SoldCount { get; set; }

		public int TotalSold { get; set; }

		public decimal SoldAmount { get; set; }

		public decimal Price { get; set; }

		public string EbayUser { get; set; }

		public DateTime LastDateSold { get; set; }
	}
}
