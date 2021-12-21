using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Messaging.Client
{
	public interface IClient
	{
		public Task<TResponse> SendMessageAsync<TRequest, TResponse>(TRequest req, int timeout = 10000)
			where TRequest : class
			where TResponse : class;
		public Task PublishMessageAsync<TRequest>(TRequest req, int timeout = 10000);
	}
}
