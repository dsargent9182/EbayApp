using DS.EbayAPI.Dto;

namespace DS.EbayAPI.BizLayer
{
	public interface IEbayService
	{
		Task<IEnumerable<GiftCardDto>> GetGiftCards(int? id, int timeout = 10000);
		Task<IEnumerable<WatchListDto>> GetWatchList(string ebayUser, int timeout = 10000);
	}
}