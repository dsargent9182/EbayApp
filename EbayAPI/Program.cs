using Ebay.Context.Dapper;
using Ebay.DataLayer;
using Ebay.Util;
using MassTransit;


var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();

string ebayConn = builder.Configuration.GetConnectionString("EbayConnection");
string fileName = builder.Configuration.GetSection("Log4Net")["FileName"];
var section = builder.Configuration.GetSection("RabbitMQ");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<DapperContext>(new DapperContext(ebayConn));
builder.Services.AddScoped<IEbayRepository, EbayRepository>();
builder.Services.AddSingleton<ILoggerManager>(new LoggerManager(fileName));
builder.Services.AddSingleton<Ebay.Messaging.Client.IClient, Ebay.Messaging.Client.Client>();
builder.Services.AddSingleton<Ebay.Messaging.SDK.WatchList>();



builder.Services.AddMassTransit(x =>
{
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
builder.Services.AddMassTransitHostedService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
