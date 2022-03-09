using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ebay.MicroService;
using Microsoft.Extensions.Logging;
using DS.Lib.Logger;
using DS.Domain.Models;

namespace Ebay.MicroService.Consumers
{
	public class MessageConsumer : IConsumer<Message>
	{
		private readonly ILoggerManager _logger;
		public MessageConsumer(ILoggerManager logger)
		{
			_logger = logger;
		}
		public Task Consume(ConsumeContext<Message> context)
		{
			_logger.LogInfo("[Start] Message consumer");
			_logger.LogInfo($"Message received: {context.Message.Text}");

			_logger.LogInfo("[ End ] Message consumer");
			return Task.CompletedTask;
		}
	}
}
