using FinancialSupport.Application.DTOs;
using FinancialSupport.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSupport.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
//    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    [Authorize] // assim permeite qualquer autorização
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IEmprestimoService _emprestimoService;

        public UsuariosController(IUsuarioService usuarioService, IEmprestimoService emprestimoService)
        {
            _usuarioService = usuarioService;
            _emprestimoService = emprestimoService; 
        }

        [HttpGet]
        public async Task<ActionResult<List<UsuarioDTO>>> Get()
        {
            var usuarios = await _usuarioService.GetUsuarios();
            if (usuarios == null)
                return NotFound("Usuários não encontrados");
            else
                return Ok(usuarios);
        }
        [HttpGet("{id:int}", Name = "GetUsuario")]
        public async Task<ActionResult<UsuarioDTO>> Get(int id)
        {
            var usuario = await _usuarioService.GetById(id);
            if (usuario == null)
                return NotFound("Usuário não encontrado");
            else
                return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UsuarioDTO usuarioDto)
        {
            if (usuarioDto == null)
                return BadRequest("Dados inválidos");

            await _usuarioService.Add(usuarioDto);

            return new CreatedAtRouteResult("GetUsuario", new { id = usuarioDto.Id }, usuarioDto);
        }

        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromBody] UsuarioDTO usuarioDto)
        {
            if (id != usuarioDto.Id) return BadRequest();

            if (usuarioDto == null) return BadRequest();

            await _usuarioService.Update(usuarioDto);

            return Ok(usuarioDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<UsuarioDTO>> Delete(int id)
        {
            var usuario = await _usuarioService.GetById(id);

            if (usuario == null) return NotFound("Usuário não encontrado");

            var emprestimo = await _emprestimoService.GetEmprestimosByIdUsuario(id);

            if (emprestimo != null) return BadRequest("Usuário tem empréstimo e não pode ser apagado");

            await _usuarioService.Remove(usuario);

            return Ok(usuario);
        }
    }
}
