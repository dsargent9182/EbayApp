using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.MicroService.WatchList
{
	public class WatchListResponse
	{
		public IEnumerable<Model.WatchList> watchLists { get; set; }
	}
}
