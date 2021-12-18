using Microsoft.AspNetCore.Mvc;

namespace EbayApp.Controllers
{
	public class SellerOverview : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
