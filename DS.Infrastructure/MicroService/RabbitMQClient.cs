using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Infrastructure.MicroService
{
	public class RabbitMQClient
	{
		private readonly IBus _bus;
		public RabbitMQClient(IBus bus)
		{
			_bus = bus;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TRequest"></typeparam>
		/// <typeparam name="TResponse"></typeparam>
		/// <param name="req"></param>
		/// <param name="timeout">Default timeout is 10 seconds</param>
		/// <returns></returns>
		public async Task<TResponse> SendMessageAsync<TRequest, TResponse>(TRequest req, int timeout = 10000)
			where TRequest : class
			where TResponse : class
		{
			var requestClient = _bus.CreateRequestClient<TRequest>();
			var resp = await requestClient.GetResponse<TResponse>(req, timeout: timeout);
			return resp.Message;
		}
		public async Task PublishMessageAsync<TRequest>(TRequest req, int timeout = 10000)
		{
			CancellationTokenSource source = new CancellationTokenSource(timeout);
			await _bus.Publish(req, source.Token);
		}
	}
}
