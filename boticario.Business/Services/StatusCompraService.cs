using boticario.Business.Interfaces;
using boticario.Helpers;
using boticario.Helpers.Enums;
using boticario.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace boticario.Services
{
    public class StatusCompraService : IRepository<StatusCompra>
    {
        private readonly AppDbContext context;
        private readonly HistoricoService historicoService;
        private readonly HelperService helperService;

        public StatusCompraService(AppDbContext context, HistoricoService historicoService, HelperService helperService)
        {
            this.context = context;
            this.historicoService = historicoService;
            this.helperService = helperService;
        }

        public async Task<StatusCompra> Create(StatusCompra entity, string usuario)
        {
            try
            {
                context.StatusCompra.Add(entity);

                await context.SaveChangesAsync();

                string json = JsonConvert.SerializeObject(entity);

                await historicoService.Create(new Historico
                {
                    ChaveTable = entity.Id,
                    NomeTabela = typeof(StatusCompra).Name,
                    JsonAntes = string.Empty,
                    JsonDepois = json,
                    Usuario = usuario,
                    IdTipoHistorico = (int)TipoHistoricoEnum.Action.Create
                });

                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteById(int id, string usuario)
        {
            StatusCompra entity = await context.StatusCompra.FindAsync(id);

            if (entity is null)
                return false;

            string json = JsonConvert.SerializeObject(entity);

            try
            {
                entity.Ativo = false;

                context.StatusCompra.Update(entity);

                await context.SaveChangesAsync();

                await historicoService.Create(new Historico
                {
                    ChaveTable = entity.Id,
                    NomeTabela = typeof(StatusCompra).Name,
                    JsonAntes = json,
                    JsonDepois = string.Empty,
                    Usuario = usuario,
                    IdTipoHistorico = (int)TipoHistoricoEnum.Action.Delete
                });

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<StatusCompra>> GetAll()
            => await context.StatusCompra.Where(item => item.Ativo).ToListAsync();

        public async Task<StatusCompra> GetById(int id)
            => await context.StatusCompra.FirstOrDefaultAsync(item => item.Id.Equals(id));

        public async Task<bool> IsExist(int id)
            => await context.StatusCompra.AnyAsync(item => item.Id.Equals(id));

        public async Task<bool> Update(StatusCompra entity, string usuario)
        {
            StatusCompra status = await helperService.GetEntityAntiga<StatusCompra>(entity.Id);
            string oldJson = JsonConvert.SerializeObject(status);

            entity.DataAlteracao = DateTime.Now;

            string newJson = JsonConvert.SerializeObject(entity);

            context.Entry(entity).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();

                await historicoService.Create(new Historico
                {
                    ChaveTable = entity.Id,
                    NomeTabela = typeof(StatusCompra).Name,
                    JsonAntes = oldJson,
                    JsonDepois = newJson,
                    Usuario = usuario,
                    IdTipoHistorico = (int)TipoHistoricoEnum.Action.Update
                });

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IsExist(entity.Id))
                    return false;

                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
