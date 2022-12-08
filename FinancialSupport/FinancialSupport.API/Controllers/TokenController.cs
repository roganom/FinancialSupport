using FinancialSupport.API.Models;
using FinancialSupport.Domain.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinancialSupport.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IAutenticate _autentication;
        private readonly IConfiguration _configuration;

        public TokenController(IAutenticate autentication, IConfiguration configuration)
        {
            _autentication = autentication ?? throw new ArgumentNullException(nameof(autentication));
            _configuration = configuration;
        }

        [HttpPost]    // APENAS PARA DOMONSTRAÇÂO
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] LoginModel userInfo)
        {
            var result = await _autentication.RegisterUser(userInfo.Email, userInfo.Password);


            if (result)
            {
                //return GenerateToken(userInfo);
                return Ok($"Usuário {userInfo.Email} criado com sucesso."); // APENAS PARA DOMONSTRAÇÂO
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Tentativa inválida de criação de usuário.");
                return BadRequest(ModelState);
            }
        }

        [AllowAnonymous]
        [HttpPost("LoginUser")]
        public async Task<ActionResult<UserToken>> Login([FromBody] LoginModel userInfo)
        {
            var result = await _autentication.Authenticate(userInfo.Email, userInfo.Password);

            if (result)
            {
                return GenerateToken(userInfo);
                //return Ok($"Usuário {userInfo.Email} logado com sucesso.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Tentativa inválida de login.");
                return BadRequest(ModelState);
            }
        }

        private UserToken GenerateToken(LoginModel userInfo)
        {
            //declarações do usuário
            var claims = new[]
            {
                new Claim("email", userInfo.Email),
                new Claim("meuValor", "o que vc quiser"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //gerar chave privada para assinar o token
            var privateKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            //gerar a assinatura digital (chave simétrica)
            var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);

            //definir o tempo de expitação do token
            var expiration = DateTime.UtcNow.AddMinutes(10);

            //gerar p token
            JwtSecurityToken token = new JwtSecurityToken(
                //emissor
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
                );

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
