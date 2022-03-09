using DS.EbayAPI.Dto;
using AutoMapper;
using DS.Domain.Contracts.GiftCard;
using DS.Domain.Contracts.WatchList;
using DS.Domain.Models;

namespace DS.EbayAPI.BizLayer
{
	public class EbayService : IEbayService
	{
		private readonly ISDK _sdk;
		private readonly IMapper _mapper;

		public EbayService(ISDK sdk, IMapper mapper)
		{
			_sdk = sdk;
			_mapper = mapper;
		}

		public async Task<IEnumerable<GiftCardDto>> GetGiftCards(int? id, int timeout = 10000)
		{
			GiftCardRequest req = new GiftCardRequest();
			req.Id = id;
			var resp = await _sdk.GiftCards_Get(req);
			if (null == resp)
			{
				throw new Exception("Unable to contact service");
			}
			IEnumerable<GiftCard> giftCards = resp.GiftCards;
			var giftcardsDto = _mapper.Map<IEnumerable<GiftCard>,IEnumerable<GiftCardDto>>(giftCards);

			return giftcardsDto;
		}
		public async Task<IEnumerable<WatchListDto>> GetWatchList(string ebayUser, int timeout = 10000)
		{
			WatchListRequest req = new WatchListRequest();
			req.EbayUser = ebayUser;
			var resp = await _sdk.WatchList_Get(req);
			if (null == resp)
			{
				throw new Exception("Unable to contact service");
			}
			IEnumerable<WatchList> watchList = resp.WatchLists;
			var watchListDto = _mapper.Map<IEnumerable<WatchList>, IEnumerable<WatchListDto>>(watchList);

			return watchListDto;
		}



	}
}
