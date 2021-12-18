namespace EbayApp.DataLayer
{
	public interface IEbayRepository
	{
		public Task<IEnumerable<WatchList>> GetWatchListAsync(DateTime? dateToRun);

		public Task<IEnumerable<GiftCard>> GetGiftCardsAsync(int? Id);

		public Task UpdateOrCreateGiftCardAsync(GiftCard gift);

	}
}
