using Ebay.MicroService.Consumers;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ebay
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) 
		{
			var builder = Host.CreateDefaultBuilder(args);

			builder.ConfigureServices((hostContext, services) =>
				{
					var config = hostContext.Configuration;
					var section = config.GetSection("RabbitMQ");
					string logFile = config.GetSection("Log4Net")["FileName"];
					string ebayConn = config.GetSection("ConnectionStrings")["EbayConnection"];
					
					services.AddMassTransit(x =>
				   {
					   x.AddConsumer<MessageConsumer>();
					   x.AddConsumer<WatchListConsumer>();

					   x.UsingRabbitMq((context, cfg) =>
					   {
						   cfg.ConfigureEndpoints(context);
						   cfg.Host(section["URL"], h =>
						   {
							   h.Username(section["UserName"]);
							   h.Password(section["Password"]);
						   });

					   });
				   });
					services.AddMassTransitHostedService();
					//services.AddHostedService<Worker>();
					services.AddSingleton<Ebay.Util.ILoggerManager>(new Ebay.Util.LoggerManager(logFile));
					services.AddSingleton<Ebay.Context.Dapper.DapperContext>(new Ebay.Context.Dapper.DapperContext(ebayConn));
					services.AddScoped<Ebay.DataLayer.IEbayRepository, Ebay.DataLayer.EbayRepository>();

				});



				return builder;
		}
	}
}
