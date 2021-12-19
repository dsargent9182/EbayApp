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
		public WatchListController(IEbayRepository ebayRepository)
		{
			_ebayRepository = ebayRepository;
		}
		[HttpGet(Name = "GetWatchList")]
		public async Task<IEnumerable<WatchList>> Get()
		{
			return await _ebayRepository.GetWatchListAsync(new DateTime(2019,10,1));
		}
	}
}
