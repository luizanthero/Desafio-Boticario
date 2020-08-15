using System;
using System.Threading.Tasks;
using boticario.Helpers.Enums;
using boticario.Models;
using boticario.Options;
using boticario.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace boticario.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CashbackController : ControllerBase
    {
        private readonly CashbackService service;
        private readonly ILogger<CashbackController> logger;

        private readonly string controllerName = nameof(CashbackController);

        public CashbackController(CashbackService service, ILogger<CashbackController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// Retorna o Cashback acumulado por CPF (Rota Desafio Boticário)
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        /// <response code="200">Sucesso ao buscar registro</response>
        /// <response code="400">Informação enviada inválida</response>
        /// <response code="401">Sem autorização</response>
        /// <response code="403">Acesso negado</response>
        /// <response code="404">Registro não encontrado</response>
        /// <response code="500">Erro Interno no servidor</response>
        [HttpGet("{cpf}")]
        public async Task<ActionResult<Cashback>> GetCashbackPoints(string cpf)
        {
            const string endpointName = nameof(GetCashbackPoints);
            string header = $"GET | {UserTokenOptions.GetClaimTypesNameValue(User.Identity)} | {controllerName}: {endpointName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Start.Value}");

                Cashback cashback = await service.GetCashbackPoints(cpf, UserTokenOptions.GetClaimTypesNameValue(User.Identity));

                if (cashback is null)
                {
                    logger.LogWarning((int)LogEventEnum.Events.GetItem,
                        $"{header} - {MessageError.NotFoundSingle.Value}");

                    return NotFound(new { message = MessageError.NotFoundSingle.Value });
                }

                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Stop.Value}");

                return Ok(cashback.Body);
            }
            catch (Exception ex)
            {
                logger.LogError((int)LogEventEnum.Events.GetItem, ex,
                    $"{header} - {MessageLog.Error.Value} | Exception: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = MessageError.InternalError.Value, error = ex.Message });
            }
        }
    }
}
