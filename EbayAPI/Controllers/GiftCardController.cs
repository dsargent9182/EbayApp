using Microsoft.AspNetCore.Mvc;
using DS.EbayAPI.BizLayer;
using DS.Lib.Logger;

namespace DS.EbayAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class GiftCardController : ControllerBase
	{

		private readonly IEbayService _ebayService;
		private readonly ILoggerManager _loggerManager;
		
		public GiftCardController(IEbayService ebayService, ILoggerManager loggerManager)
		{
			_loggerManager = loggerManager;
			_ebayService = ebayService;
		}
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				IEnumerable<Dto.GiftCardDto> giftCardDto = await _ebayService.GetGiftCards(null);
				_loggerManager.LogInfo("[Success] Gift Cards returned");

				return Ok(giftCardDto);
			}
			catch (Exception ex)
			{
				_loggerManager.LogInfo(ex.Message);
				return StatusCode(500, ex.Message);
			}
		}

		// GET api/<GiftCardController>/5
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			//await _bus.Publish<Ebay.MicroService.Message>(new Ebay.MicroService.Message { Text = "hello from api" });
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
