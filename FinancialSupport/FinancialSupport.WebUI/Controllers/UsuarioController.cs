using FinancialSupport.Application.DTOs;
using FinancialSupport.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSupport.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IEmprestimoService _emprestimoService;
        private readonly IParcelaService _parcelaService;
        private readonly IWebHostEnvironment _environment;
        public UsuarioController(IUsuarioService usuarioService, IWebHostEnvironment environment, IEmprestimoService emprestimo, IParcelaService parcelaService)
        {
            _usuarioService = usuarioService;
            _parcelaService = parcelaService;
            _emprestimoService = emprestimo;
            _environment = environment;
        }
        #region Indice
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuarios = await _usuarioService.GetUsuarios();
            return View(usuarios);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UsuarioDTO usuario)
        {
            if (ModelState.IsValid)
            {
                await _usuarioService.Add(usuario);
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }
        #endregion

        #region Edit
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var usuarioDto = await _usuarioService.GetById(id);

            if (usuarioDto == null) return NotFound();

            return View(usuarioDto);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(UsuarioDTO usuarioDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _usuarioService.Update(usuarioDto);
                }
                catch (Exception)  // colocar "catch (Exception ex)" e mostar ex (mensagem de exceção
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(usuarioDto);
        }
        #endregion

        #region Delete
        [Authorize(Roles = "Admin")]
        [HttpGet()]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var usuarioDto = await _usuarioService.GetById(id);

            if (usuarioDto == null) return NotFound();

            return View(usuarioDto);
        }

        [HttpPost(), ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _usuarioService.Remove(id);
            return RedirectToAction("Index");
        }
        #endregion

    }
}
