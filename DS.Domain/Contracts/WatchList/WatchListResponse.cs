using DS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Domain.Contracts.WatchList
{
	public class WatchListResponse
	{
		public IEnumerable<DS.Domain.Models.WatchList> WatchLists { get; set; }
	}
}
