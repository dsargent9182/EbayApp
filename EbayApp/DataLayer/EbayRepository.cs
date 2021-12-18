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

		public async Task<IEnumerable<GiftCard>> GetGiftCardsAsync(int? Id)
		{
			IEnumerable<GiftCard> giftCards = new List<GiftCard>();
			using (var connection = _context.CreateConnection())
			{
				
				giftCards = await connection.QueryAsync<GiftCard>("GiftCard_Get", new {Id = Id }, commandType: System.Data.CommandType.StoredProcedure);
			}
			return giftCards;
		}

		public async Task<IEnumerable<WatchList>> GetWatchListAsync(DateTime? dateToRun)
		{
			object values = null;
			IEnumerable<WatchList> watchLists = new List<WatchList>();

			if (dateToRun.HasValue)
				values = new { end = dateToRun };

			using (var connection = _context.CreateConnection())
			{
				watchLists = await connection.QueryAsync<WatchList>("SnipeList_Get", values, commandType: System.Data.CommandType.StoredProcedure);
			}
			return watchLists;
		}

		public async Task UpdateOrCreateGiftCardAsync(GiftCard gift)
		{
			object values = new { Id = gift.Id, Store = gift.Store, Number = gift.Number, PIN = gift.PIN, Amount = gift.Amount, Balance = gift.Balance };
		
			using (var connection = _context.CreateConnection())
			{
				var obj = await connection.ExecuteScalarAsync("GiftCard_Update", values, commandType: System.Data.CommandType.StoredProcedure);
			}
		}

		public async Task<IEnumerable<Message>> GetMessagesAsync(string itemId, string buyerUserName)
		{
			IEnumerable<Message> messages = new List<Message>();

			using (var connection = _context.CreateConnection())
			{
				messages = await connection.QueryAsync<Message>("Message_Get", null, commandType: System.Data.CommandType.StoredProcedure);
			}
			return messages;
		}

	}
}
