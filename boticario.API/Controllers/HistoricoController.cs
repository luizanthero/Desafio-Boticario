using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using boticario.Helpers.Enums;
using boticario.Options;
using boticario.Services;
using boticario.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace boticario.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HistoricoController : ControllerBase
    {
        private readonly HistoricoService service;
        private readonly ILogger<HistoricoController> logger;

        private readonly string controllerName = nameof(HistoricoController);

        public HistoricoController(HistoricoService service, ILogger<HistoricoController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// Retorna todos os Históricos
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso ao buscar todos os registros</response>
        /// <response code="400">Informação enviada inválida</response>
        /// <response code="401">Sem autorização</response>
        /// <response code="403">Acesso negado</response>
        /// <response code="404">Registro não encontrado</response>
        /// <response code="500">Erro Interno no servidor</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistoricoViewModel>>> GetAll()
        {
            const string endpointName = nameof(GetAll);
            string header = $"GET | {UserTokenOptions.GetClaimTypesNameValue(User.Identity)} | {controllerName}: {endpointName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Start.Value}");

                IEnumerable<HistoricoViewModel> entities = await service.GetAll(UserTokenOptions.GetClaimTypesNameValue(User.Identity));

                if (entities.ToList().Count <= 0)
                {
                    logger.LogInformation((int)LogEventEnum.Events.GetItemNotFound,
                        $"{header} - {MessageError.NotFound.Value}");

                    return NotFound(new { message = MessageError.NotFound.Value });
                }

                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Stop.Value}");

                return Ok(entities);
            }
            catch (Exception ex)
            {
                logger.LogError((int)LogEventEnum.Events.GetItemError, ex,
                    $"{header} - {MessageLog.Error.Value} | Exception: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = MessageError.InternalError.Value, error = ex.Message });
            }
        }

        /// <summary>
        /// Retorna um Histórico por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Sucesso ao buscar registro</response>
        /// <response code="400">Informação enviada inválida</response>
        /// <response code="401">Sem autorização</response>
        /// <response code="403">Acesso negado</response>
        /// <response code="404">Registro não encontrado</response>
        /// <response code="500">Erro Interno no servidor</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<HistoricoViewModel>> GetById(int id)
        {
            const string endpointName = nameof(GetById);
            string header = $"GET | {UserTokenOptions.GetClaimTypesNameValue(User.Identity)} | {controllerName}: {endpointName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Start.Value}");

                HistoricoViewModel entity = await service.GetById(id, UserTokenOptions.GetClaimTypesNameValue(User.Identity));

                if (entity is null)
                {
                    logger.LogInformation((int)LogEventEnum.Events.GetItemNotFound,
                        $"{header} - {MessageError.NotFoundSingle.Value}");

                    return NotFound(new { message = MessageError.NotFoundSingle.Value });
                }

                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Stop.Value}");

                return Ok(entity);
            }
            catch (Exception ex)
            {
                logger.LogError((int)LogEventEnum.Events.GetItemError, ex,
                    $"{header} - {MessageLog.Error.Value} | Exception: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = MessageError.InternalError.Value, error = ex.Message });
            }
        }

        /// <summary>
        /// Retorna todos os Históricos pelo Nome da Tabela
        /// </summary>
        /// <param name="nomeTabela"></param>
        /// <returns></returns>
        /// <response code="200">Sucesso ao buscar todos os registros</response>
        /// <response code="400">Informação enviada inválida</response>
        /// <response code="401">Sem autorização</response>
        /// <response code="403">Acesso negado</response>
        /// <response code="404">Registro não encontrado</response>
        /// <response code="500">Erro Interno no servidor</response>
        [HttpGet("NomeTabela/{nomeTabela}")]
        public async Task<ActionResult<IEnumerable<HistoricoViewModel>>> GetByNomeTabela(string nomeTabela)
        {
            const string endpointName = nameof(GetByNomeTabela);
            string header = $"GET | {UserTokenOptions.GetClaimTypesNameValue(User.Identity)} | {controllerName}: {endpointName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Start.Value}");

                IEnumerable<HistoricoViewModel> entities = await service.GetByNomeTabela(nomeTabela, 
                    UserTokenOptions.GetClaimTypesNameValue(User.Identity));

                if (entities is null)
                {
                    logger.LogInformation((int)LogEventEnum.Events.GetItemNotFound,
                        $"{header} - {MessageError.NotFoundSingle.Value}");

                    return NotFound(new { message = MessageError.NotFoundSingle.Value });
                }

                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Stop.Value}");

                return Ok(entities);
            }
            catch (Exception ex)
            {
                logger.LogError((int)LogEventEnum.Events.GetItemError, ex,
                    $"{header} - {MessageLog.Error.Value} | Exception: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = MessageError.InternalError.Value, error = ex.Message });
            }
        }

        /// <summary>
        /// Retorna todos os Históricos pela Chave da Tabela
        /// </summary>
        /// <param name="chaveTabela"></param>
        /// <returns></returns>
        /// <response code="200">Sucesso ao buscar todos os registros</response>
        /// <response code="400">Informação enviada inválida</response>
        /// <response code="401">Sem autorização</response>
        /// <response code="403">Acesso negado</response>
        /// <response code="404">Registro não encontrado</response>
        /// <response code="500">Erro Interno no servidor</response>
        [HttpGet("ChaveTabela/{chaveTabela}")]
        public async Task<ActionResult<IEnumerable<HistoricoViewModel>>> GetByChaveTabela(int chaveTabela)
        {
            const string endpointName = nameof(GetByChaveTabela);
            string header = $"GET | {UserTokenOptions.GetClaimTypesNameValue(User.Identity)} | {controllerName}: {endpointName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Start.Value}");

                IEnumerable<HistoricoViewModel> entities = await service.GetByChaveTabela(chaveTabela,
                    UserTokenOptions.GetClaimTypesNameValue(User.Identity));

                if (entities is null)
                {
                    logger.LogInformation((int)LogEventEnum.Events.GetItemNotFound,
                        $"{header} - {MessageError.NotFoundSingle.Value}");

                    return NotFound(new { message = MessageError.NotFoundSingle.Value });
                }

                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Stop.Value}");

                return Ok(entities);
            }
            catch (Exception ex)
            {
                logger.LogError((int)LogEventEnum.Events.GetItemError, ex,
                    $"{header} - {MessageLog.Error.Value} | Exception: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = MessageError.InternalError.Value, error = ex.Message });
            }
        }

        /// <summary>
        /// Retorna todos os Históricos pelo Nome e Chave da Tabela
        /// </summary>
        /// <param name="nomeTabela"></param>
        /// <param name="chaveTabela"></param>
        /// <returns></returns>
        /// <response code="200">Sucesso ao buscar todos os registros</response>
        /// <response code="400">Informação enviada inválida</response>
        /// <response code="401">Sem autorização</response>
        /// <response code="403">Acesso negado</response>
        /// <response code="404">Registro não encontrado</response>
        /// <response code="500">Erro Interno no servidor</response>
        [HttpGet("{nomeTabela}/{chaveTabela}")]
        public async Task<ActionResult<IEnumerable<HistoricoViewModel>>> GetByTabelaChave(string nomeTabela, int chaveTabela)
        {
            const string endpointName = nameof(GetByTabelaChave);
            string header = $"GET | {UserTokenOptions.GetClaimTypesNameValue(User.Identity)} | {controllerName}: {endpointName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Start.Value}");

                IEnumerable<HistoricoViewModel> entities = await service.GetByTabelaChave(nomeTabela, chaveTabela,
                    UserTokenOptions.GetClaimTypesNameValue(User.Identity));

                if (entities is null)
                {
                    logger.LogInformation((int)LogEventEnum.Events.GetItemNotFound,
                        $"{header} - {MessageError.NotFoundSingle.Value}");

                    return NotFound(new { message = MessageError.NotFoundSingle.Value });
                }

                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Stop.Value}");

                return Ok(entities);
            }
            catch (Exception ex)
            {
                logger.LogError((int)LogEventEnum.Events.GetItemError, ex,
                    $"{header} - {MessageLog.Error.Value} | Exception: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = MessageError.InternalError.Value, error = ex.Message });
            }
        }
    }
}
