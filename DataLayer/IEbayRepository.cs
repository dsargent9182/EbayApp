using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using W = Ebay.MicroService.WatchList;

namespace Ebay.DataLayer
{
	public interface IEbayRepository
	{
		public Task<IEnumerable<W.Model.WatchList>> GetWatchListAsync(DateTime? dateToRun);

		public Task<IEnumerable<GiftCard>> GetGiftCardsAsync(int? Id);

		public Task UpdateOrCreateGiftCardAsync(GiftCard gift);

		public Task<IEnumerable<Message>> GetMessagesAsync(string itemId, string buyerUserName);

	}
}
