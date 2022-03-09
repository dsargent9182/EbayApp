using DS.Domain.Contracts.GiftCard;
using DS.Domain.Contracts.WatchList;

namespace DS.EbayAPI.BizLayer
{
	public interface ISDK
	{
		Task<GiftCardResponse> GiftCards_Get(GiftCardRequest req, int timeout = 10000);
		Task<WatchListResponse> WatchList_Get(WatchListRequest req, int timeout = 10000);
	}
}
