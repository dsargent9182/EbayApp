using DS.Lib.Logger;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DS.Ebay.MicroService.Infrastructure.Repositories;
using DS.Domain.Contracts.GiftCard;

namespace Ebay.MicroService.Consumers
{
	public class GiftCardConsumer : IConsumer<GiftCardRequest>
	{
		private readonly ILoggerManager _logger;
		private readonly IEbayRepository _ebayRepository;

		public GiftCardConsumer(ILoggerManager loggerManager, IEbayRepository ebayRepository)
		{
			_logger = loggerManager;
			_ebayRepository = ebayRepository;
		}
		public async Task Consume(ConsumeContext<GiftCardRequest> context)
		{
			_logger.LogInfo("[Start] Gift Cards Get");
			GiftCardResponse resp = new GiftCardResponse();
			try
			{
				Int32? id = context.Message.Id;
				var giftCards = await _ebayRepository.GetGiftCardsAsync(id);
				resp.GiftCards = giftCards; 
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);
			}
			
			_logger.LogInfo("[ End ] Gift Cards Get");
			context.Respond(resp);
		}
	}
}
