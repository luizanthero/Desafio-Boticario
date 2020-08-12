using boticario.Business.Interfaces;
using boticario.Helpers;
using boticario.Helpers.Enums;
using boticario.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boticario.Services
{
    public class RevendedorService : IRepository<Revendedor>
    {
        private readonly AppDbContext context;
        private readonly HistoricoService historicoService;
        private readonly HelperService helperService;

        public RevendedorService(AppDbContext context, HistoricoService historicoService, HelperService helperService)
        {
            this.context = context;
            this.historicoService = historicoService;
            this.helperService = helperService;
        }

        public async Task<Revendedor> Create(Revendedor entity, string usuario)
        {
            try
            {
                context.Revendedores.Add(entity);

                await context.SaveChangesAsync();

                string json = JsonConvert.SerializeObject(entity);

                await historicoService.Create(new Historico
                {
                    ChaveTable = entity.Id,
                    NomeTabela = typeof(Revendedor).Name,
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
            Revendedor entity = await context.Revendedores.FindAsync(id);

            if (entity is null)
                return false;

            string json = JsonConvert.SerializeObject(entity);

            try
            {
                entity.Ativo = false;

                context.Revendedores.Update(entity);

                await context.SaveChangesAsync();

                await historicoService.Create(new Historico
                {
                    ChaveTable = entity.Id,
                    NomeTabela = typeof(Revendedor).Name,
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

        public async Task<IEnumerable<Revendedor>> GetAll()
            => await context.Revendedores.Where(item => item.Ativo).ToListAsync();

        public async Task<Revendedor> GetById(int id)
            => await context.Revendedores.FirstOrDefaultAsync(item => item.Id.Equals(id));

        public async Task<bool> IsExist(int id)
            => await context.Revendedores.AnyAsync(item => item.Id.Equals(id));

        public async Task<bool> Update(Revendedor entity, string usuario)
        {
            Revendedor revendedor = await helperService.GetEntityAntiga<Revendedor>(entity.Id);
            string oldJson = JsonConvert.SerializeObject(revendedor);

            entity.DataAlteracao = DateTime.Now;

            string newJson = JsonConvert.SerializeObject(entity);

            context.Entry(entity).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();

                await historicoService.Create(new Historico
                {
                    ChaveTable = entity.Id,
                    NomeTabela = typeof(Revendedor).Name,
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
