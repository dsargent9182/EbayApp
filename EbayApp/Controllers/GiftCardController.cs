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
			var result = await _ebayRepository.GetGiftCardsAsync(null);

			return View(result);
		}
		[HttpGet]
		public async Task<IActionResult> Edit(int Id)
		{
			var result = await _ebayRepository.GetGiftCardsAsync(Id);

			return View(result.FirstOrDefault());
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int Id,[Bind("Id,Store,Number,PIN,Amount,Balance")] GiftCard giftCard)
		{
			if (Id != giftCard.Id)
				throw new InvalidDataException("Access denied for this gift card");

			if(ModelState.IsValid)
			{
				await _ebayRepository.UpdateOrCreateGiftCardAsync(giftCard);
				return RedirectToAction("Index");
			}

			return View(giftCard);
		}


	}
}
