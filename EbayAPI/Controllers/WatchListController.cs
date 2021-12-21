using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using C = Ebay.Messaging.Client;
using Ebay.DataLayer;
using W = Ebay.MicroService.WatchList;
using MassTransit;
using Ebay.Messaging.Client;

namespace EbayAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class WatchListController : ControllerBase
	{
		private readonly ILogger _logger;
		private readonly Ebay.Messaging.SDK.WatchList _watchList;
		public WatchListController(ILogger<WatchList> logger,Ebay.Messaging.SDK.WatchList watchList)
		{
			_logger = logger;
			_watchList = watchList;
		}
		[HttpGet(Name = "GetWatchList")]
		public async Task<IResult> Get()
		{
			try
			{
				W.WatchListRequest req = new W.WatchListRequest();
				req.StartDate = new DateTime(2019, 10, 01);

				var resp = await _watchList.GetWatchList(req,1000);
				
				return Results.Ok(resp.watchLists);

			}
			catch(Exception ex)
			{
				_logger.LogError(ex.Message);
				ProblemDetails problemDetails = new ProblemDetails();
			
				return Results.Problem("Unable to get watchlist. Try again","CoolInstance",500,"MessageTimeOut","PoopType");
			}
		}
	}
}
