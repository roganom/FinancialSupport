using FinancialSupport.Application.DTOs;
using FinancialSupport.Application.Interfaces;
using FinancialSupport.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSupport.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ParcelaController : Controller
    {
        private readonly IParcelaService _parcelaService;
        public ParcelaController(IParcelaService parcelaService)
        {
            _parcelaService = parcelaService;
        }
        #region Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var parcelas = await _parcelaService.GetParcelas();

            return View(parcelas);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        #endregion

        #region Create
        [HttpPost]
        public async Task<IActionResult> Create(ParcelaDTO parcela)
        {
            if (ModelState.IsValid)
            {
                parcela.DataPagamento = parcela.DataPagamento != null && parcela.DataPagamento != DateTime.Parse("0001-01-01") ? parcela.DataPagamento : DateTime.Parse("1900-01-01");

                await _parcelaService.Add(parcela);
                return RedirectToAction(nameof(Index));
            }
            return View(parcela);
        }
        #endregion

        #region Edit
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var parcelaDto = await _parcelaService.GetById(id);

            if (parcelaDto == null) return NotFound();

            return View(parcelaDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ParcelaDTO parcelaDto)
        {
            if (ModelState.IsValid)
            {
                parcelaDto.DataPagamento = parcelaDto.DataPagamento != null && parcelaDto.DataPagamento != DateTime.Parse("0001-01-01") ? parcelaDto.DataPagamento : DateTime.Parse("1900-01-01");

                try
                {
                    await _parcelaService.Update(parcelaDto);
                }
                catch (Exception)  // colocar "catch (Exception ex)" e mostar ex (mensagem de exceção
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(parcelaDto);
        }
        #endregion

        #region Delete
        [Authorize(Roles = "Admin")]
        [HttpGet()]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var parcelaDto = await _parcelaService.GetById(id);

            if (parcelaDto == null) return NotFound();

            return View(parcelaDto);
        }

        [HttpPost(), ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            await _parcelaService.Remove(id);
            return RedirectToAction("Index");
        }
        #endregion
    }
}
