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
    public class RegraCashbackService : IRepository<RegraCashback>
    {
        private readonly AppDbContext context;
        private readonly HistoricoService historicoService;
        private readonly HelperService helperService;

        public RegraCashbackService(AppDbContext context, HistoricoService historicoService, HelperService helperService)
        {
            this.context = context;
            this.historicoService = historicoService;
            this.helperService = helperService;
        }

        public async Task<RegraCashback> Create(RegraCashback entity, string usuario)
        {
            try
            {
                context.RegrasCashback.Add(entity);

                await context.SaveChangesAsync();

                string json = JsonConvert.SerializeObject(entity);

                await historicoService.Create(new Historico
                {
                    ChaveTabela = entity.Id,
                    NomeTabela = typeof(RegraCashback).Name,
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
            RegraCashback entity = await context.RegrasCashback.FindAsync(id);

            if (entity is null)
                return false;

            string json = JsonConvert.SerializeObject(entity);

            try
            {
                entity.Ativo = false;

                context.RegrasCashback.Update(entity);

                await context.SaveChangesAsync();

                await historicoService.Create(new Historico
                {
                    ChaveTabela = entity.Id,
                    NomeTabela = typeof(RegraCashback).Name,
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

        public async Task<IEnumerable<RegraCashback>> GetAll()
            => await context.RegrasCashback.Where(item => (bool)item.Ativo).ToListAsync();

        public async Task<RegraCashback> GetById(int id)
            => await context.RegrasCashback.FirstOrDefaultAsync(item => item.Id.Equals(id));

        public async Task<bool> IsExist(int id)
            => await context.RegrasCashback.AnyAsync(item => item.Id.Equals(id));

        public async Task<bool> Update(RegraCashback entity, string usuario)
        {
            RegraCashback regra = await helperService.GetEntityAntiga<RegraCashback>(entity.Id);
            string oldJson = JsonConvert.SerializeObject(regra);

            entity.DataCriacao = regra.DataCriacao;
            entity.DataAlteracao = DateTime.Now;

            string newJson = JsonConvert.SerializeObject(entity);

            context.Entry(entity).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();

                await historicoService.Create(new Historico
                {
                    ChaveTabela = entity.Id,
                    NomeTabela = typeof(RegraCashback).Name,
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
