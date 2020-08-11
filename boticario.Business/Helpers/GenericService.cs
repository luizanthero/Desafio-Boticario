using boticario.Context;
using boticario.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace boticario.Helpers
{
    public class GenericService
    {
        private readonly AppDbContext context;

        public GenericService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<T> GetEntityAntiga<T>(int id) where T : class
        {
            try
            {
                if (typeof(T).Equals(typeof(Compra)))
                    return await context.Compras.AsNoTracking().FirstOrDefaultAsync(item => item.Id.Equals(id)) as T;

                if (typeof(T).Equals(typeof(Historico)))
                    return await context.Historicos.AsNoTracking().FirstOrDefaultAsync(item => item.Id.Equals(id)) as T;

                if (typeof(T).Equals(typeof(ParametroSistema)))
                    return await context.ParametrosSistema.AsNoTracking().FirstOrDefaultAsync(item => item.Id.Equals(id)) as T;

                if (typeof(T).Equals(typeof(RegraCashback)))
                    return await context.RegrasCashback.AsNoTracking().FirstOrDefaultAsync(item => item.Id.Equals(id)) as T;

                if (typeof(T).Equals(typeof(Revendedor)))
                    return await context.Revendedores.AsNoTracking().FirstOrDefaultAsync(item => item.Id.Equals(id)) as T;

                if (typeof(T).Equals(typeof(StatusCompra)))
                    return await context.StatusCompra.AsNoTracking().FirstOrDefaultAsync(item => item.Id.Equals(id)) as T;

                if (typeof(T).Equals(typeof(TipoHistorico)))
                    return await context.TiposHistorico.AsNoTracking().FirstOrDefaultAsync(item => item.Id.Equals(id)) as T;

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
