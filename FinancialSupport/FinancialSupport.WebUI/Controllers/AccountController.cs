using FinancialSupport.Domain.Account;
using FinancialSupport.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSupport.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAutenticate _autentication;
        public AccountController(IAutenticate autentication)
        {
            _autentication = autentication;
        }

        [HttpGet]
        public IActionResult Register() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var result = await _autentication.RegisterUser(model.Email, model.Password);
            
            if (result)
            {
                return Redirect("/");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Tentativa de registro inválida (a senha deve ser forte).");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel()
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model) 
        {
            var result = await _autentication.Authenticate(model.Email, model.Password);
            
            if (result)
            {
                if (string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }

                return Redirect(model.ReturnUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Tentativa de login inválida.");
                return View (model);
            }
        }
        public async Task<IActionResult> Logout()
        {
            await _autentication.Logout();
            return Redirect("/Account/Login");
        }
    }
}
 