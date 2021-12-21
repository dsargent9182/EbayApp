using Ebay.Messaging.Client;
using Ebay.MicroService.WatchList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Messaging.SDK
{
	public class WatchList
	{
		private readonly IClient _client;
		public WatchList(IClient client)
		{
			_client = client;
		}
		public async Task<WatchListResponse> GetWatchList(WatchListRequest req, int timeout = 10000/*,int? ttl, int? taskTimeout*/)
		{
			return await _client.SendMessageAsync<WatchListRequest, WatchListResponse>(req, timeout);
		}
	}
}
