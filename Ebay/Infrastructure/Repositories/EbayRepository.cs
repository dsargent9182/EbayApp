using Dapper;
using DS.Domain.Models;
using DS.Ebay.MicroService.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Ebay.MicroService.Infrastructure.Repositories
{
	public class EbayRepository : IEbayRepository
	{
		private readonly IDatabaseContext _context;
		public EbayRepository(IDatabaseContext context)
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
