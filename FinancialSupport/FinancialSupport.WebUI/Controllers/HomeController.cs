using Microsoft.AspNetCore.Mvc;

namespace FinancialSupport.WebUI.Controllers
{
    public class HomeController : Controller
    {
        // original
        public IActionResult Index()
        {
            return View();
        }
    }
}
