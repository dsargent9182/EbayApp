using MassTransit;
using DS.Lib.Logger;
using DS.EbayAPI.BizLayer;
using DS.Infrastructure.MicroService;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

string ebayConn = builder.Configuration.GetConnectionString("EbayConnection");
string fileName = builder.Configuration.GetSection("Log4Net")["FileName"];
var section = builder.Configuration.GetSection("RabbitMQ");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IClient, RabbitMQClient>();
builder.Services.AddSingleton<ISDK,SDK>();
builder.Services.AddSingleton<IEbayService,EbayService>();
builder.Services.AddSingleton<ILoggerManager>(new Log4NetManager(fileName));

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
	app.UseCors(builder =>
		builder.WithOrigins("http://127.0.0.1:5500"));
}
else
{
	//Turn this on if not in development
	//This causes issues with CORS for fetch api
	app.UseHttpsRedirection();
}


app.UseAuthorization();

app.MapControllers();

app.Run();
