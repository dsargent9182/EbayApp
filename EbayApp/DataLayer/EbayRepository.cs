using EbayApp.Context.Dapper;
using Dapper;

namespace EbayApp.DataLayer
{
	public class EbayRepository : IEbayRepository
	{
		private readonly DapperContext _context;
		public EbayRepository(DapperContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<WatchList>> GetWatchListAsync(DateTime? dateToRun)
		{
			object values = null;
			IEnumerable<WatchList> watchLists = new List<WatchList>();

			if(dateToRun.HasValue) 
				values = new { end = dateToRun };

			using (var connection = _context.CreateConnection() )
			{
				watchLists = await connection.QueryAsync<WatchList>("SnipeList_Get",values,commandType: System.Data.CommandType.StoredProcedure);
			}
			return watchLists;
		}
	}
}
