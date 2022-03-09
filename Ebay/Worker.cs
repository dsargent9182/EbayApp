using DS.Domain.Models;
using Ebay.MicroService;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ebay
{
	public class Worker : BackgroundService
	{
		private readonly IBus _bus;
		public Worker(IBus bus)
		{
			_bus = bus;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				await _bus.Publish(new Message { Text = $"I'm sending this at {DateTime.Now}"});

				await Task.Delay(1000,stoppingToken);
			}
		}
	}
}
