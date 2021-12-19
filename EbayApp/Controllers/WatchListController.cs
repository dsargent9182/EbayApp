using Ebay.Context.Dapper;
using Ebay.DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EbayApp.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WatchListController : Controller
	{
		private IEbayRepository _ebayRepository;

		public WatchListController(IEbayRepository ebayRepository)
		{
			_ebayRepository = ebayRepository;
		}

		[HttpGet]
		public async Task<ActionResult> Get()
		{
			var list = await _ebayRepository.GetWatchListAsync(new DateTime(2019,10,01));
			return View(list);
		}

		// GET: WatchListController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: WatchListController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: WatchListController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: WatchListController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: WatchListController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: WatchListController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: WatchListController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
