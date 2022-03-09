using DS.Domain.Contracts.WatchList;
using DS.Lib.Logger;
using DS.Ebay.MicroService.Infrastructure.Repositories;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.MicroService.Consumers
{
	public class WatchListConsumer : IConsumer<WatchListRequest>
	{
		private readonly ILoggerManager _logger;
		private readonly IEbayRepository _ebayRepository;
		public WatchListConsumer(ILoggerManager logger, IEbayRepository ebayRepository)
		{
			_logger = logger;
			_ebayRepository = ebayRepository;
		}
		public async Task Consume(ConsumeContext<WatchListRequest> context)
		{
			_logger.LogInfo("[Start] Watch List Get");
			WatchListResponse resp = new WatchListResponse();
			try
			{
				DateTime? startDate = context.Message.StartDate;
				var watchList = await _ebayRepository.GetWatchListAsync(startDate);
				resp.WatchLists = watchList;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);
			}
			_logger.LogInfo("[ End ] Watch List Get");
			context.Respond(resp);
		}
	}
}
