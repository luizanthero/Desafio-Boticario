using boticario.Business.Interfaces;
using boticario.Context;
using boticario.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace boticario.Services
{
    public class HistoricoService
    {
        public readonly AppDbContext context;

        public HistoricoService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Historico> Create(Historico entity)
        {
            try
            {
                context.Historicos.Add(entity);

                await context.SaveChangesAsync();

                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteById(int id)
        {
            Historico historico = await context.Historicos.FindAsync(id);

            if (historico is null)
                return false;

            try
            {
                context.Historicos.Remove(historico);

                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Historico>> GetAll()
            => await context.Historicos.ToListAsync();

        public async Task<Historico> GetById(int id)
            => await context.Historicos.FirstOrDefaultAsync(item => item.Id.Equals(id));

        public async Task<bool> IsExist(int id)
            => await context.Historicos.AnyAsync(item => item.Id.Equals(id));

        public async Task<bool> Update(Historico entity)
        {
            context.Entry(entity).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();

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
