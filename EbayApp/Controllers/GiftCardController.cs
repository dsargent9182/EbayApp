using EbayApp.DataLayer;
using Microsoft.AspNetCore.Mvc;

namespace EbayApp.Controllers
{
	public class GiftCardController : Controller
	{
		private IEbayRepository _ebayRepository;
		public GiftCardController(IEbayRepository ebayRepository)
		{
			_ebayRepository = ebayRepository; 
		}
		public async Task<IActionResult> Index()
		{
			var result = await _ebayRepository.GetGiftCardsAsync();
		
			return View(result);
		}
	}
}
