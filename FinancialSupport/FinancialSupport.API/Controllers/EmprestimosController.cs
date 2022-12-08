using FinancialSupport.Application.DTOs;
using FinancialSupport.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSupport.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmprestimosController : ControllerBase
    {
        private readonly IEmprestimoService _emprestimoService;
        private readonly IParcelaService _parcelaService;

        public EmprestimosController(IEmprestimoService emprestimoService, IParcelaService parcelaService)
        {
            _emprestimoService = emprestimoService;
            _parcelaService = parcelaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<EmprestimoDTO>>> Get()
        {
            var emprestimo = await _emprestimoService.GetEmprestimos();
            if (emprestimo == null)
                return NotFound("Empréstimos não encontrados");
            else
                return Ok(emprestimo);
        }
        [HttpGet("{id:int}", Name = "GetEmprestimo")]
        public async Task<ActionResult<EmprestimoDTO>> Get(int id)
        {
            var emprestimo = await _emprestimoService.GetEmprestimoById(id);
            if (emprestimo == null)
                return NotFound("Empréstimo não encontrado");
            else
                return Ok(emprestimo);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] EmprestimoDTO emprestimoDto)
        {
            if (emprestimoDto == null)
                return BadRequest("Dados inválidos");

            await _emprestimoService.Add(emprestimoDto);

            return new CreatedAtRouteResult("GetEmprestimo", new { id = emprestimoDto.Id }, emprestimoDto);
        }

        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromBody] EmprestimoDTO emprestimoDto)
        {
            if (id != emprestimoDto.Id) return BadRequest();

            if (emprestimoDto == null) return BadRequest();

            await _emprestimoService.Update(emprestimoDto);

            return Ok(emprestimoDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<EmprestimoDTO>> Delete(int id)
        {
            var emprestimo = await _emprestimoService.GetEmprestimoById(id);

            if (emprestimo == null) return NotFound("Empréstimo não encontrado");

            var parcela = await _parcelaService.GetByIdEmprestimo(id);

            if (parcela != null) return BadRequest("Empréstimo tem parcela e não pode ser apagado");

            await _emprestimoService.Remove(id);

            return Ok(emprestimo);
        }
    }
}
