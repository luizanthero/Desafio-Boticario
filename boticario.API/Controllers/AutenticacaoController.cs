using System;
using System.Threading.Tasks;
using boticario.Helpers.Enums;
using boticario.Models;
using boticario.Services;
using boticario.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace boticario.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly RevendedorService service;
        private readonly ILogger<AutenticacaoController> logger;

        private readonly string controllerName = nameof(AutenticacaoController);

        public AutenticacaoController(RevendedorService service, ILogger<AutenticacaoController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// Registra um novo Revendedor no Sistema (Rota Desafio Boticário)
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
            const string endpointName = nameof(Register);

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.InsertItem, 
                    $"{revendedor.Email} | {controllerName}: {endpointName} - {MessageLog.Start.Value}");

                Revendedor result = await service.Register(revendedor);

                logger.LogInformation((int)LogEventEnum.Events.InsertItem,
                    $"{revendedor.Email} | {controllerName}: {endpointName} - {MessageLog.Stop.Value}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError((int)LogEventEnum.Events.InsertItemError, ex, 
                    $"{revendedor.Email} | {controllerName}: {endpointName} - {MessageLog.Error.Value} | Exception: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = MessageError.InternalError.Value, error = ex.Message });
            }
        }

        /// <summary>
        /// Autentica um Revendedor (Rota Desafio Boticário)
        /// </summary>
        /// <param name="auth"></param>
        /// <returns></returns>
        /// <response code="200">Revendedor autenticado</response>
        /// <response code="400">Informação enviada inválida</response>
        /// <response code="500">Erro Interno no servidor</response>
        [HttpPost("authenticate")]
        public async Task<ActionResult<string>> Authentication(AuthenticationViewModel auth)
        {
            const string endpointName = nameof(Authentication);
            try
            {
                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{auth.Email} | {controllerName}: {endpointName} - {MessageLog.Start.Value}");

                string token = await service.Authentication(auth.Email, auth.Senha);

                if (string.IsNullOrEmpty(token))
                {
                    logger.LogWarning((int)LogEventEnum.Events.GetItem,
                        $"{auth.Email} | {controllerName}: {endpointName} - {MessageError.UserPasswordInvalid.Value}");

                    return BadRequest(new { message = MessageError.UserPasswordInvalid.Value });
                }

                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{auth.Email} | {controllerName}: {endpointName} - {MessageLog.Stop.Value} | Token: {token}");

                return Ok(token);
            }
            catch (Exception ex)
            {
                logger.LogError((int)LogEventEnum.Events.GetItemError, ex,
                    $"{auth.Email} | {controllerName}: {endpointName} - {MessageLog.Error.Value} | Exception: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = MessageError.InternalError.Value, error = ex.Message });
            }
        }
    }
}
