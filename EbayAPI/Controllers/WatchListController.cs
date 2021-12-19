using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Ebay.DataLayer;

namespace EbayAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class WatchListController : ControllerBase
	{
		private IEbayRepository _ebayRepository;
		private ILogger _logger;
		public WatchListController(IEbayRepository ebayRepository, ILogger<WatchList> logger)
		{
			_ebayRepository = ebayRepository;
			_logger = logger;
		}
		[HttpGet(Name = "GetWatchList")]
		public async Task<IActionResult> Get()
		{
			try{
				throw new ApplicationException("Unable to get this");
				var watchList =  await _ebayRepository.GetWatchListAsync(new DateTime(2019, 10, 1));
				return Ok(watchList);

			}
			catch(Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(500, ex.Message);
			}
	
		}
	}
}
