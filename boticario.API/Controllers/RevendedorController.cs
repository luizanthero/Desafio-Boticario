﻿using System;
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

namespace boticario.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RevendedorController : ControllerBase, IController<Revendedor>
    {
        private readonly RevendedorService service;

        public RevendedorController(RevendedorService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Exclui um Revendedor por Id
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
            try
            {
                string usuario = UserTokenOptions.GetClaimTypesNameValue(User.Identity);

                if (await service.DeleteById(id, usuario))
                    return Ok(new { message = MessageSuccess.Delete.Value });

                return BadRequest(new { message = MessageError.BadRequest });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = MessageError.InternalError.Value, error = ex.Message });
            }
        }

        /// <summary>
        /// Retorna todos os Revendedores(as)
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso ao buscar todos os registros</response>
        /// <response code="400">Informação enviada inválida</response>
        /// <response code="401">Sem autorização</response>
        /// <response code="403">Acesso negado</response>
        /// <response code="404">Registro não encontrado</response>
        /// <response code="500">Erro Interno no servidor</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Revendedor>>> GetAll()
        {
            try
            {
                IEnumerable<Revendedor> entities = await service.GetAll();

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
        /// Retorna um Revendedor por Id
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
        public async Task<ActionResult<Revendedor>> GetById(int id)
        {
            try
            {
                Revendedor entity = await service.GetById(id);

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
        /// Criação de um novo Revendedor
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
        public async Task<ActionResult<Revendedor>> Post(Revendedor entity)
        {
            try
            {
                string usuario = UserTokenOptions.GetClaimTypesNameValue(User.Identity);

                entity = await service.Create(entity, usuario);

                return Ok(entity);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = MessageError.InternalError.Value, error = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um Revendedor por Id
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
        public async Task<IActionResult> Put(int id, Revendedor entity)
        {
            try
            {
                if (id != entity.Id)
                    return BadRequest(new { message = MessageError.DifferentIds.Value });

                string usuario = UserTokenOptions.GetClaimTypesNameValue(User.Identity);

                if (await service.Update(entity, usuario))
                    return Ok(new { message = MessageSuccess.Update.Value });

                return NotFound(new { message = MessageError.NotFoundSingle.Value });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = MessageError.InternalError.Value, error = ex.Message });
            }
        }
    }
}
