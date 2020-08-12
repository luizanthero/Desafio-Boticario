using System;
using System.Threading.Tasks;
using boticario.Helpers.Enums;
using boticario.Models;
using boticario.Services;
using boticario.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace boticario.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly RevendedorService service;

        public AutenticacaoController(RevendedorService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Registra um novo Revendedor no Sistema
        /// </summary>
        /// <param name="revendedor"></param>
        /// <returns></returns>
        /// <remarks>
        /// Regras dos campos:
        /// 
        ///  - CPF - apenas números
        /// </remarks>
        /// <response code="200">Revendedor Registrado</response>
        /// <response code="400">Informação enviada inválida</response>
        /// <response code="500">Erro Interno no servidor</response>
        [HttpPost("register")]
        public async Task<ActionResult<Revendedor>> Register(Revendedor revendedor)
        {
            try
            {
                Revendedor result = await service.Register(revendedor);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = MessageError.InternalError.Value, error = ex.Message });
            }
        }

        /// <summary>
        /// Autentica um Revendedor
        /// </summary>
        /// <param name="auth"></param>
        /// <returns></returns>
        /// <response code="200">Revendedor autenticado</response>
        /// <response code="400">Informação enviada inválida</response>
        /// <response code="500">Erro Interno no servidor</response>
        [HttpPost("authenticate")]
        public async Task<ActionResult<string>> Authentication(AuthenticationViewModel auth)
        {
            try
            {
                var token = await service.Authentication(auth.Email, auth.Senha);

                if (string.IsNullOrEmpty(token))
                    return BadRequest(new { message = MessageError.UserPasswordInvalid.Value });

                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = MessageError.InternalError.Value, error = ex.Message });
            }
        }
    }
}
