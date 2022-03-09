using DS.Lib.Logger;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DS.Ebay.MicroService.Infrastructure.Context;
using Ebay.MicroService.Consumers;
using DS.Ebay.MicroService.Infrastructure.Repositories;


namespace DS.Ebay
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
					   x.AddConsumer<GiftCardConsumer>();

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
					services.AddSingleton<ILoggerManager>(new Log4NetManager(logFile));
					services.AddSingleton<IDatabaseContext>(new EbayContext(ebayConn));
					services.AddScoped<IEbayRepository, EbayRepository>();

				});



				return builder;
		}
	}
}
