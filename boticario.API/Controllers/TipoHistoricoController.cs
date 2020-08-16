using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using boticario.API.Interfaces;
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
    public class TipoHistoricoController : ControllerBase, IController<TipoHistorico>
    {
        private readonly TipoHistoricoService service;
        private readonly ILogger<TipoHistoricoController> logger;

        private readonly string controllerName = nameof(TipoHistoricoController);

        public TipoHistoricoController(TipoHistoricoService service, ILogger<TipoHistoricoController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// Exclui um Tipo de Histórico por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Sucesso ao excluir registro</response>
        /// <response code="400">Informação enviada inválida</response>
        /// <response code="401">Sem autorização</response>
        /// <response code="403">Acesso negado</response>
        /// <response code="404">Registro não encontrado</response>
        /// <response code="500">Erro Interno no servidor</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            string usuario = UserTokenOptions.GetClaimTypesNameValue(User.Identity);

            const string endpointName = nameof(Delete);
            string header = $"DELETE | {usuario} | {controllerName}: {endpointName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.DeleteItem,
                    $"{header} - {MessageLog.Start.Value}");

                if (await service.DeleteById(id, usuario))
                {
                    logger.LogInformation((int)LogEventEnum.Events.DeleteItem,
                        $"{header} - {MessageLog.Stop.Value}");

                    return Ok(new { message = MessageSuccess.Delete.Value });
                }

                logger.LogWarning((int)LogEventEnum.Events.DeleteItemNotFound,
                    $"{header} - {MessageError.BadRequest.Value}");

                return BadRequest(new { message = MessageError.BadRequest.Value });
            }
            catch (Exception ex)
            {
                logger.LogError((int)LogEventEnum.Events.DeleteItemError,
                    $"{header} - {MessageLog.Error.Value} | Exception: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = MessageError.InternalError.Value, error = ex.Message });
            }
        }

        /// <summary>
        /// Retorna todos os Tipos de Histórico
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso ao buscar todos os registros</response>
        /// <response code="400">Informação enviada inválida</response>
        /// <response code="401">Sem autorização</response>
        /// <response code="403">Acesso negado</response>
        /// <response code="404">Registro não encontrado</response>
        /// <response code="500">Erro Interno no servidor</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoHistorico>>> GetAll()
        {
            const string endpointName = nameof(GetAll);
            string header = $"GET | {UserTokenOptions.GetClaimTypesNameValue(User.Identity)} | {controllerName}: {endpointName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Start.Value}");

                IEnumerable<TipoHistorico> entities = await service.GetAll(UserTokenOptions.GetClaimTypesNameValue(User.Identity));

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
        /// Retorna um Tipo de Histórico por Id
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
        public async Task<ActionResult<TipoHistorico>> GetById(int id)
        {
            const string endpointName = nameof(GetById);
            string header = $"GET | {UserTokenOptions.GetClaimTypesNameValue(User.Identity)} | {controllerName}: {endpointName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Start.Value}");

                TipoHistorico entity = await service.GetById(id, UserTokenOptions.GetClaimTypesNameValue(User.Identity));

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
        /// Criação de um novo Tipo Histórico
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <response code="200">Sucesso ao criar registro</response>
        /// <response code="400">Informação enviada inválida</response>
        /// <response code="401">Sem autorização</response>
        /// <response code="403">Acesso negado</response>
        /// <response code="404">Registro não encontrado</response>
        /// <response code="500">Erro Interno no servidor</response>
        [HttpPost]
        public async Task<ActionResult<TipoHistorico>> Post(TipoHistorico entity)
        {
            string usuario = UserTokenOptions.GetClaimTypesNameValue(User.Identity);

            const string endpointName = nameof(Post);
            string header = $"POST | {usuario} | {controllerName}: {endpointName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.InsertItem,
                    $"{header} | {MessageLog.Start.Value}");

                entity = await service.Create(entity, usuario);

                logger.LogInformation((int)LogEventEnum.Events.InsertItem,
                    $"{header} | {MessageLog.Stop.Value}");

                return Ok(entity);
            }
            catch (Exception ex)
            {
                logger.LogError((int)LogEventEnum.Events.InsertItemError,
                    $"{header} | {MessageLog.Error.Value} | Exception: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = MessageError.InternalError.Value, error = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um Tipo de Histórico por Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <response code="200">Sucesso ao atualizar registro</response>
        /// <response code="400">Informação enviada inválida</response>
        /// <response code="401">Sem autorização</response>
        /// <response code="403">Acesso negado</response>
        /// <response code="404">Registro não encontrado</response>
        /// <response code="500">Erro Interno no servidor</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, TipoHistorico entity)
        {
            string usuario = UserTokenOptions.GetClaimTypesNameValue(User.Identity);

            const string endpointName = nameof(Put);
            string header = $"PUT | {usuario} | {controllerName}: {endpointName}";

            try
            {
                if (id != entity.Id)
                {
                    logger.LogWarning((int)LogEventEnum.Events.UpdateItemNotFound,
                        $"{MessageError.DifferentIds.Value} - ID Entrada: {id} | ID Objeto: {entity.Id}");

                    return BadRequest(new { message = MessageError.DifferentIds.Value });
                }

                logger.LogInformation((int)LogEventEnum.Events.UpdateItem,
                    $"{header} - {MessageLog.Start.Value}");

                if (await service.Update(entity, usuario))
                {
                    logger.LogInformation((int)LogEventEnum.Events.UpdateItem,
                        $"{header} - {MessageLog.Stop.Value}");

                    return Ok(new { message = MessageSuccess.Update.Value });
                }

                logger.LogWarning((int)LogEventEnum.Events.UpdateItemNotFound,
                    $"{header} - {MessageLog.UpdateNotFound.Value} - ID: {id}");

                return NotFound(new { message = MessageError.NotFoundSingle.Value });
            }
            catch (Exception ex)
            {
                logger.LogError((int)LogEventEnum.Events.UpdateItemError, ex,
                    $"{header} - {MessageLog.Error.Value} - Exception: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = MessageError.InternalError.Value, error = ex.Message });
            }
        }
    }
}
