using FinancialSupport.Application.DTOs;
using FinancialSupport.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSupport.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParcelasController : ControllerBase
    {
        private readonly IParcelaService _parcelaService;

        public ParcelasController(IParcelaService parcelaService)
        {
            _parcelaService = parcelaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ParcelaDTO>>> Get()
        {
            var parcela = await _parcelaService.GetParcelas();
            if (parcela == null)
                return NotFound("Parcelas não encontradas");
            else
                return Ok(parcela);
        }
        [HttpGet("{id:int}", Name = "GetParcela")]
        public async Task<ActionResult<ParcelaDTO>> Get(int id)
        {
            var parcela = await _parcelaService.GetById(id);
            if (parcela == null)
                return NotFound("Parcela não encontrada");
            else
                return Ok(parcela);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ParcelaDTO parcelaDto)
        {
            if (parcelaDto == null)
                return BadRequest("Dados inválidos");

            await _parcelaService.Add(parcelaDto);

            return new CreatedAtRouteResult("GetParcela", new { id = parcelaDto.Id }, parcelaDto);
        }

        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromBody] ParcelaDTO parcelaDto)
        {
            if (id != parcelaDto.Id) return BadRequest();

            if (parcelaDto == null) return BadRequest();

            await _parcelaService.Update(parcelaDto);

            return Ok(parcelaDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ParcelaDTO>> Delete(int id)
        {
            var parcela = await _parcelaService.GetById(id);

            if (parcela == null) return NotFound("Parcela não encontrada");

            await _parcelaService.Remove(parcela);

            return Ok(parcela);
        }
    }
}
