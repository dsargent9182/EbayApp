using DS.Domain.Contracts.WatchList;
using DS.Domain.Models;
using DS.EbayAPI.BizLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace EbayAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class WatchListController : ControllerBase
	{
		private readonly ILogger _logger;
		private readonly IEbayService _ebayService;
		public WatchListController(ILogger<WatchList> logger, IEbayService ebayService)
		{
			_logger = logger;
			_ebayService = ebayService;
		}
		public async Task<IResult> Get()
		{
			try
			{
				
				var resp = await _ebayService.GetWatchList(null);
				
				return Results.Ok(resp);

			}
			catch(Exception ex)
			{
				_logger.LogError(ex.Message);
				return Results.Problem("Unable to get watchlist. Try again",null,500,"MessageTimeOut");
			}
		}
	}
}
