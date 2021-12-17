namespace EbayApp.DataLayer
{
	public interface IEbayRepository
	{
		public Task<IEnumerable<WatchList>> GetWatchListAsync(DateTime? dateToRun);
	}
}
