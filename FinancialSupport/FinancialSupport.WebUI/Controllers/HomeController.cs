using Microsoft.AspNetCore.Mvc;

namespace FinancialSupport.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
