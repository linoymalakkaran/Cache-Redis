using System.Web.Mvc;

namespace svc_global_cache.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Title = "Home Page";
			return View();
		}
	}
}
