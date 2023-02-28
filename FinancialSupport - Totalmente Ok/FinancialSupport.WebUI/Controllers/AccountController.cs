using FinancialSupport.Domain.Account;
using FinancialSupport.WebUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinancialSupport.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAutenticate _autentication;
        public AccountController(IAutenticate autentication)
        {
            _autentication = autentication;
        }

        #region Consulta lista de usuários
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Consulta(int? id, int? tipoMensagem, string? mensagem)
        {
            if (tipoMensagem == null)
                tipoMensagem = 0;

            if (mensagem == null)
                mensagem = "";

            var model = new List<ConsultaLoginViewModel>();
            var result = await _autentication.ListaUsuarios();

            foreach (var item in result)
            {
                var viewModel = new ConsultaLoginViewModel();

                viewModel.User = item.User;
                viewModel.Role = item.Role;
                viewModel.Bloqueio = item.Bloqueio;
                model.Add(viewModel);
            }

            model[0].CustomMessagePartial.TipoMensagem = (int)tipoMensagem;
            model[0].CustomMessagePartial.Mensagem = mensagem;

            return View(model);
        }
        #endregion

        #region Cadastra(registra) novo usuário
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            model.CustomMessagePartial.TipoMensagem = 2;
            model.CustomMessagePartial.Mensagem = "A senha deve ter no mínimo 10 caracteres, com maiúscula, números e caracteres especiais.";

            ViewBag.Tipo = new SelectList(new List<string>() { "USER", "ADMIN" }, "Tipo");

            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var result = await _autentication.RegisterUser(model.Email, model.Password, model.Tipo);

            if (result)
            {
                model.CustomMessagePartial.TipoMensagem = 1;
                model.CustomMessagePartial.Mensagem = "Usuário criado com sucesso.";
                return View(model);
                //return RedirectToAction("/", model);
                //return Redirect("/");
            }
            else
            {
                model.CustomMessagePartial.TipoMensagem = 3;
                model.CustomMessagePartial.Mensagem = "Usuário não foi criado. Verifique as regras de formação da senha.";
                //                ModelState.AddModelError(string.Empty, "Tentativa de registro inválida. Verifiqu as regras de formação da senha.");
                return View(model);
            }
        }
        #endregion

        #region Troca de senha
        [Authorize]
        [HttpGet]
        public IActionResult TrocaSenha()
        {
            TrocaSenhaViewModel model = new TrocaSenhaViewModel();
            model.CustomMessagePartial.TipoMensagem = 2;
            model.CustomMessagePartial.Mensagem = "A senha deve ter no mínimo 10 caracteres, com maiúscula, números e caracteres especiais.";
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> TrocaSenha(TrocaSenhaViewModel model)
        {
            var result = await _autentication.TrocaSenhaUser(model.Email, model.OldPassword, model.NewPassword);

            if (result)
            {
                model.CustomMessagePartial.TipoMensagem = 1;
                model.CustomMessagePartial.Mensagem = "Senha trocada com sucesso.";
            }
            else
            {
                model.CustomMessagePartial.TipoMensagem = 3;
                model.CustomMessagePartial.Mensagem = "Não foi possível trocar a senha do usuário. Verifique a senha atual informada e a regra de formação para a nova senha.";
            }
            return View(model);
        }
        #endregion

        #region Login
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel() { ReturnUrl = returnUrl });
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
                return View(model);
            }
        }
        #endregion

        #region Logout
        public async Task<IActionResult> Logout()
        {
            await _autentication.Logout();
            return RedirectToAction("Index", "Home");
            //return Redirect("/FinancialSupport/Account/Login");
        }
        #endregion

        #region Bloqueio
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Bloqueia(string User)
        {
            int tipoMensagem = 3;
            string mensagem = "Não foi possível bloquear este usuário.";

            if (User != null)
            {
                var result = await _autentication.LockUser(User);
                if (result)
                {
                    tipoMensagem = 1;
                    mensagem = "Usuário bloqueado com sucesso.";
                }
            }
            return RedirectToAction("Consulta", new { tipoMensagem = tipoMensagem, mensagem = mensagem });
        }
        #endregion

        #region Desbloqueio
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Desbloqueia(string? User)
        {
            int tipoMensagem = 3;
            string mensagem = "Não foi possível desbloquear este usuário.";

            if (User != null)
            {
                var result = await _autentication.UnlockUser(User);
                if (result)
                {
                    tipoMensagem = 1;
                    mensagem = "Usuário desbloqueado com sucesso.";
                }
            }
            return RedirectToAction("Consulta", new { tipoMensagem = tipoMensagem, mensagem = mensagem });
        }
        #endregion

        #region Reset de Senha
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetSenha(string? User)
        {
            int tipoMensagem = 3;
            string mensagem = "Não foi possível reiniciar a senha deste usuário.";

            if (User != null)
            {
                string newPassword = "SenhaNova@1234";
                var result = await _autentication.ResetPassword(User, newPassword);
                if (result)
                {
                    tipoMensagem = 1;
                    mensagem = "Senha reiniciada com sucesso: SenhaNova@1234 .";
                }
            }
            return RedirectToAction("Consulta", new { tipoMensagem = tipoMensagem, mensagem = mensagem });
        }
        #endregion

        #region Exclusão de usuário
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExcluiUsuario(string? User)
        {
            int tipoMensagem = 3;
            string mensagem = "Não foi possível excluir o usuário " + User + " .";

            if (User != null)
            {
                var result = await _autentication.DeleteUser(User);
                if (result)
                {
                    tipoMensagem = 1;
                    mensagem = "Usuário " + User + " excluído com sucesso.";
                }
            }
            return RedirectToAction("Consulta", new { tipoMensagem = tipoMensagem, mensagem = mensagem });
        }
        #endregion
    }
}
