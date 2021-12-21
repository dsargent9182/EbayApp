using Ebay.Util;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M = Ebay.MicroService;

namespace Ebay.MicroService.Consumers
{
	public class WatchListConsumer : IConsumer<M.WatchList.WatchListRequest>
	{
		private readonly ILoggerManager _logger;
		private readonly Ebay.DataLayer.IEbayRepository _ebayRepository;
		public WatchListConsumer(ILoggerManager logger, Ebay.DataLayer.IEbayRepository ebayRepository)
		{
			_logger = logger;
			_ebayRepository = ebayRepository;
		}
		public async Task Consume(ConsumeContext<M.WatchList.WatchListRequest> context)
		{
			_logger.LogInfo("[Start] Watch List Get");
			M.WatchList.WatchListResponse resp = new WatchList.WatchListResponse();
			try
			{
				DateTime? startDate = context.Message.StartDate;
				var taskList = await _ebayRepository.GetWatchListAsync(startDate);
				resp.watchLists = taskList;
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
