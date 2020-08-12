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
    public class ParametroSistemaService : IRepository<ParametroSistema>
    {
        private readonly AppDbContext context;
        private readonly HistoricoService historicoService;
        private readonly HelperService helperService;

        public ParametroSistemaService(AppDbContext context, HistoricoService historicoService, HelperService helperService)
        {
            this.context = context;
            this.historicoService = historicoService;
            this.helperService = helperService;
        }

        public async Task<ParametroSistema> Create(ParametroSistema entity, string usuario)
        {
            try
            {
                context.ParametrosSistema.Add(entity);

                await context.SaveChangesAsync();

                string json = JsonConvert.SerializeObject(entity);

                await historicoService.Create(new Historico
                {
                    ChaveTable = entity.Id,
                    NomeTabela = typeof(ParametroSistema).Name,
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
            ParametroSistema entity = await context.ParametrosSistema.FindAsync(id);

            if (entity is null)
                return false;

            string json = JsonConvert.SerializeObject(entity);

            try
            {
                entity.Ativo = false;

                await context.SaveChangesAsync();

                await historicoService.Create(new Historico
                {
                    ChaveTable = entity.Id,
                    NomeTabela = typeof(ParametroSistema).Name,
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

        public async Task<IEnumerable<ParametroSistema>> GetAll()
            => await context.ParametrosSistema.Where(item => item.Ativo).ToListAsync();

        public async Task<ParametroSistema> GetById(int id)
            => await context.ParametrosSistema.FirstOrDefaultAsync(item => item.Id.Equals(id));

        public async Task<bool> IsExist(int id)
            => await context.ParametrosSistema.AnyAsync(item => item.Id.Equals(id));

        public async Task<bool> Update(ParametroSistema entity, string usuario)
        {
            ParametroSistema parametro = await helperService.GetEntityAntiga<ParametroSistema>(entity.Id);
            string oldJson = JsonConvert.SerializeObject(parametro);

            entity.DataAlteracao = DateTime.Now;

            string newJson = JsonConvert.SerializeObject(entity);

            context.Entry(entity).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();

                await historicoService.Create(new Historico
                {
                    ChaveTable = entity.Id,
                    NomeTabela = typeof(ParametroSistema).Name,
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
