using FinancialSupport.Application.DTOs;
using FinancialSupport.Application.Interfaces;
using FinancialSupport.WebUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinancialSupport.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmprestimoController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IEmprestimoService _emprestimoService;
        private readonly IParcelaService _parcelaService;

        public EmprestimoController(IEmprestimoService emprestimoService,
                                    IParcelaService parcelaService,
                                    IUsuarioService usuarioService)
        {

            _usuarioService = usuarioService;
            _emprestimoService = emprestimoService;
            _parcelaService = parcelaService;
        }

        #region Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = new List<ListaAdmEmprestimoViewModel>();
            var emprestimos = await _emprestimoService.GetEmprestimos();
            
            foreach (var item in emprestimos)
            {
                var model = new ListaAdmEmprestimoViewModel();

                model.Id = item.Id;
                model.IdUsuario = item.IdUsuario;
                model.Valor = item.Valor;
                model.Data = item.Data;
                model.Ativo = item.Ativo;

                var usuarioDTO = await _usuarioService.GetById(item.IdUsuario);
                if (usuarioDTO != null)
                    model.NomeUsuario = usuarioDTO.Nome;
                else
                    model.NomeUsuario = "";

                viewModel.Add(model);
            }

            return View(viewModel);
        }
        #endregion

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.IdUsuario = new SelectList(await _usuarioService.GetUsuarios(), "Id", "Nome");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmprestimoDTO emprestimo)
        {
            if (ModelState.IsValid)
            {
                emprestimo.Data = (emprestimo.Data != DateTime.Parse("0001-01-01")) ? emprestimo.Data : DateTime.Parse("1900-01-01");
                emprestimo.Ativo = true;
                emprestimo.Valendo = true;
                emprestimo.DataCriacao = DateTime.Now;
                emprestimo.NumeroParcelas = 60;

                if (emprestimo.Valor > 0)
                {
                    await _emprestimoService.Add(emprestimo);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(emprestimo);
        }
        #endregion

        #region Edit
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var emprestimoDto = await _emprestimoService.GetEmprestimoById(id);

            if (emprestimoDto == null) return NotFound();

            return View(emprestimoDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmprestimoDTO emprestimoDto)
        {
            if (ModelState.IsValid)
            {
                emprestimoDto.Data = (emprestimoDto.Data != DateTime.Parse("0001-01-01")) ? emprestimoDto.Data : DateTime.Parse("1900-01-01");

                try
                {
                    await _emprestimoService.UpdateAdm(emprestimoDto);
                }
                catch (Exception)  // colocar "catch (Exception ex)" e mostar ex (mensagem de exceção
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(emprestimoDto);
        }
        #endregion

        #region Delete
        [HttpGet()]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var emprestimoDto = await _emprestimoService.GetEmprestimoById(id);

            if (emprestimoDto == null) return NotFound();

            return View(emprestimoDto);
        }

        [HttpPost(), ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id, string UsuarioAlteracao)
        {
            var emprestimoDto = await _emprestimoService.GetEmprestimoById(id);

            if (emprestimoDto == null) NotFound();

            emprestimoDto.UsuarioAlteracao = UsuarioAlteracao;
            await _emprestimoService.RemoveAdm(emprestimoDto);
            return RedirectToAction("Index");
        }
        #endregion
    }
}
