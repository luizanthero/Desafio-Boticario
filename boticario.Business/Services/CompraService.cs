using boticario.Business.Interfaces;
using boticario.Context;
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
    public class CompraService : IRepository<Compra>
    {
        private readonly AppDbContext context;
        private readonly HistoricoService historicoService;
        private readonly HelperService helperService;

        public CompraService(AppDbContext context, HistoricoService historicoService, HelperService helperService)
        {
            this.context = context;
            this.historicoService = historicoService;
            this.helperService = helperService;
        }

        public async Task<Compra> Create(Compra entity, string usuario)
        {
            try
            {
                context.Compras.Add(entity);

                await context.SaveChangesAsync();

                string json = JsonConvert.SerializeObject(entity);

                await historicoService.Create(new Historico
                {
                    ChaveTable = entity.Id,
                    NomeTabela = typeof(Compra).Name,
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
            Compra entity = await context.Compras.FindAsync(id);

            string json = JsonConvert.SerializeObject(entity);

            if (entity is null)
                return false;

            try
            {
                entity.Ativo = false;

                context.Compras.Update(entity);

                await context.SaveChangesAsync();

                await historicoService.Create(new Historico
                {
                    ChaveTable = entity.Id,
                    NomeTabela = typeof(Compra).Name,
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

        public async Task<IEnumerable<Compra>> GetAll()
            => await context.Compras.Where(item => item.Ativo).ToListAsync();

        public async Task<Compra> GetById(int id)
            => await context.Compras.FirstOrDefaultAsync(item => item.Id.Equals(id));

        public async Task<bool> IsExist(int id)
            => await context.Compras.AnyAsync(item => item.Id.Equals(id));

        public async Task<bool> Update(Compra entity, string usuario)
        {
            Compra compra = await helperService.GetEntityAntiga<Compra>(entity.Id);
            string oldJson = JsonConvert.SerializeObject(compra);

            string newJson = JsonConvert.SerializeObject(entity);

            entity.DataAlteracao = DateTime.Now;

            context.Entry(entity).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();

                await historicoService.Create(new Historico
                {
                    ChaveTable = entity.Id,
                    NomeTabela = typeof(Compra).Name,
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
