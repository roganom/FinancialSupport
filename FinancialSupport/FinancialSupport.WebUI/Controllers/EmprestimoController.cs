using FinancialSupport.Application.DTOs;
using FinancialSupport.Application.Interfaces;
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
        public EmprestimoController(IEmprestimoService emprestimoService, IParcelaService parcelaService, IUsuarioService usuarioService)
        {

            _usuarioService = usuarioService;
            _emprestimoService = emprestimoService;
            _parcelaService = parcelaService;
        }

        #region Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var emprestimos = await _emprestimoService.GetEmprestimos();
            return View(emprestimos);
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
                //                emprestimo.Data = (emprestimo.Data != null && emprestimo.Data != DateTime.Parse("0001-01-01")) ? emprestimo.Data : DateTime.Parse("1900-01-01");
                emprestimo.Data = (emprestimo.Data != DateTime.Parse("0001-01-01")) ? emprestimo.Data : DateTime.Parse("1900-01-01");
                emprestimo.Ativo = true;
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

        [Authorize(Roles = "Admin")]
        [HttpPost]

        public async Task<IActionResult> Edit(EmprestimoDTO emprestimoDto)
        {
            if (ModelState.IsValid)
            {
                //emprestimoDto.Data = (emprestimoDto.Data != null && emprestimoDto.Data != DateTime.Parse("0001-01-01")) ? emprestimoDto.Data : DateTime.Parse("1900-01-01");
                emprestimoDto.Data = (emprestimoDto.Data != DateTime.Parse("0001-01-01")) ? emprestimoDto.Data : DateTime.Parse("1900-01-01");

                try
                {
                    await _emprestimoService.Update(emprestimoDto);
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

            var parcelaDto = await _parcelaService.GetByIdEmprestimo(id);

            if (parcelaDto != null) return NotFound("Impossível apagar emprestimo que contém parcelas.");

            return View(emprestimoDto);
        }

        [HttpPost(), ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var parcelaDto = await _parcelaService.GetByIdEmprestimo(id);

            if (parcelaDto == null)
            {
                await _emprestimoService.Remove(id);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToPage("\\Mensagem.cshtml");
            }
        }
        #endregion
    }
}
