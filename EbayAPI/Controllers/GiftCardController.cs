using Microsoft.AspNetCore.Mvc;
using Ebay.DataLayer;
using MassTransit;

namespace EbayAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class GiftCardController : ControllerBase
	{
		private IEbayRepository _ebayRepository;
		private ILogger _logger;
		private Ebay.Util.ILoggerManager _loggerManager;
		private IBus _bus;
		public GiftCardController(IEbayRepository ebayRepository, ILogger<GiftCard> logger, Ebay.Util.ILoggerManager loggerManager,IBus bus)
		{
			_ebayRepository = ebayRepository;
			_logger = logger;
			_loggerManager = loggerManager;
			_bus = bus;
		}
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				var giftCards = await _ebayRepository.GetGiftCardsAsync(null);
				_loggerManager.LogInfo("[Success] Gift Cards returned");
				return Ok(giftCards);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				_loggerManager.LogInfo(ex.Message);
				return StatusCode(500, ex.Message);
			}
		}

		// GET api/<GiftCardController>/5
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			await _bus.Publish<Ebay.MicroService.Message>(new Ebay.MicroService.Message { Text = "hello from api" });
			return Ok();
		}

		// POST api/<GiftCardController>
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT api/<GiftCardController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/<GiftCardController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
