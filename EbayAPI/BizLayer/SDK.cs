using DS.Domain.Contracts.GiftCard;
using DS.Domain.Contracts.WatchList;
using DS.Infrastructure.MicroService;

namespace DS.EbayAPI.BizLayer
{
	public class SDK : ISDK
	{
		private readonly IClient _client;
		public SDK(IClient client)
		{
			_client = client;
		}
		public async Task<WatchListResponse> WatchList_Get(WatchListRequest req, int timeout = 10000/*,int? ttl, int? taskTimeout*/)
		{
			return await _client.SendMessageAsync<WatchListRequest, WatchListResponse>(req, timeout);
		}
		public async Task<GiftCardResponse> GiftCards_Get(GiftCardRequest req, int timeout = 10000/*,int? ttl, int? taskTimeout*/)
		{
			return await _client.SendMessageAsync<GiftCardRequest, GiftCardResponse>(req, timeout);
		}
	}
}
