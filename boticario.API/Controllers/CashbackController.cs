using System;
using System.Threading.Tasks;
using boticario.Helpers.Enums;
using boticario.Models;
using boticario.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace boticario.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CashbackController : ControllerBase
    {
        private readonly CashbackService service;

        public CashbackController(CashbackService service)
        {
            this.service = service;
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
            try
            {
                Cashback cashback = await service.GetCashbackPoints(cpf);

                if (cashback is null)
                    return NotFound(new { message = MessageError.NotFoundSingle.Value });

                return Ok(cashback.Body);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = MessageError.InternalError.Value, error = ex.Message });
            }
        }
    }
}
