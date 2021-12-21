using Ebay.Context.Dapper;
using Dapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using W = Ebay.MicroService.WatchList;

namespace Ebay.DataLayer
{
	public class EbayRepository : IEbayRepository
	{
		private readonly DapperContext _context;
		public EbayRepository(DapperContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<GiftCard>> GetGiftCardsAsync(int? id)
		{
			IEnumerable<GiftCard> giftCards = new List<GiftCard>();
			using (var connection = _context.CreateConnection())
			{
				
				giftCards = await connection.QueryAsync<GiftCard>("GiftCard_Get", new { Id = id }, commandType: System.Data.CommandType.StoredProcedure);
			}
			return giftCards;
		}

		public async Task<IEnumerable<W.Model.WatchList>> GetWatchListAsync(DateTime? dateToRun)
		{
			object values = null;
			IEnumerable<W.Model.WatchList> watchLists = new List<W.Model.WatchList>();

			if (dateToRun.HasValue)
				values = new { end = dateToRun };

			using (var connection = _context.CreateConnection())
			{
				watchLists = await connection.QueryAsync<W.Model.WatchList> ("SnipeList_Get", values, commandType: System.Data.CommandType.StoredProcedure);
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
