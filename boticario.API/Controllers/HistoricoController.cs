using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using boticario.Helpers.Enums;
using boticario.Models;
using boticario.Services;
using boticario.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace boticario.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class HistoricoController : ControllerBase
    {
        private readonly HistoricoService service;

        public HistoricoController(HistoricoService service)
        {
            this.service = service;
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
            try
            {
                IEnumerable<HistoricoViewModel> entities = await service.GetAll();

                if (entities.ToList().Count <= 0)
                    return NotFound(new { message = MessageError.NotFound.Value });

                return Ok(entities);
            }
            catch (Exception ex)
            {
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
            try
            {
                HistoricoViewModel entity = await service.GetById(id);

                if (entity is null)
                    return NotFound(new { message = MessageError.NotFoundSingle.Value });

                return Ok(entity);
            }
            catch (Exception ex)
            {
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
            try
            {
                IEnumerable<HistoricoViewModel> entities = await service.GetByNomeTabela(nomeTabela);

                if (entities is null)
                    return NotFound(new { message = MessageError.NotFoundSingle.Value });

                return Ok(entities);
            }
            catch (Exception ex)
            {
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
            try
            {
                IEnumerable<HistoricoViewModel> entities = await service.GetByChaveTabela(chaveTabela);

                if (entities is null)
                    return NotFound(new { message = MessageError.NotFoundSingle.Value });

                return Ok(entities);
            }
            catch (Exception ex)
            {
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
            try
            {
                IEnumerable<HistoricoViewModel> entities = await service.GetByTabelaChave(nomeTabela, chaveTabela);

                if (entities is null)
                    return NotFound(new { message = MessageError.NotFoundSingle.Value });

                return Ok(entities);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = MessageError.InternalError.Value, error = ex.Message });
            }
        }
    }
}
