using boticario.Business;
using boticario.Helpers;
using boticario.Helpers.Enums;
using boticario.Models;
using boticario.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace boticario.Services
{
    public class CompraService
    {
        private readonly AppDbContext context;
        private readonly HistoricoService historicoService;
        private readonly HelperService helperService;

        private readonly RegrasCompra regrasCompra;
        private object email;

        public CompraService(AppDbContext context, HistoricoService historicoService, HelperService helperService, 
            RegrasCompra regrasCompra)
        {
            this.context = context;
            this.historicoService = historicoService;
            this.helperService = helperService;
            this.regrasCompra = regrasCompra;
        }

        public async Task<Compra> Create(Compra entity, string usuario)
        {
            try
            {
                Revendedor revendedor = await context.Revendedores.AsNoTracking()
                    .SingleOrDefaultAsync(item => item.Email.Equals(usuario));

                if (revendedor is null)
                    return null;

                entity.IdRevendedor = revendedor.Id;

                entity.IdStatus = await regrasCompra.GetStatusCompraId(entity.CpfRevendedor);

                entity.PercentualCashback = await regrasCompra.GetPercentualCashback(entity.Valor);

                entity.ValorCashback = (entity.PercentualCashback / 100) * entity.Valor;

                context.Compras.Add(entity);

                await context.SaveChangesAsync();

                string json = JsonConvert.SerializeObject(entity);

                await historicoService.Create(new Historico
                {
                    ChaveTabela = entity.Id,
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

            if (entity is null)
                return false;

            string json = JsonConvert.SerializeObject(entity);

            try
            {
                entity.Ativo = false;

                context.Compras.Update(entity);

                await context.SaveChangesAsync();

                await historicoService.Create(new Historico
                {
                    ChaveTabela = entity.Id,
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

        public async Task<IEnumerable<CompraViewModel>> GetAll()
            => await context.Compras
                .Include(item => item.Revendedor).Include(item => item.Status)
                .Where(item => (bool)item.Ativo).Select(item => new CompraViewModel
                {
                    Codigo = item.Codigo,
                    Valor = item.Valor,
                    Data = item.DataCompra,
                    PercentualCashback = item.PercentualCashback,
                    ValorCashback = item.ValorCashback,
                    Status = item.Status.Descricao,
                    CpfUsadoCompra = item.CpfRevendedor,
                    NomeRevendedor = item.Revendedor.Nome,
                    CpfRevendedor = item.Revendedor.Cpf
                }).ToListAsync();

        public async Task<CompraViewModel> GetById(int id)
            => await context.Compras
                .Include(item => item.Revendedor).Include(item => item.Status)
                .Where(item => item.Id.Equals(id)).Select(item => new CompraViewModel
                {
                    Codigo = item.Codigo,
                    Valor = item.Valor,
                    Data = item.DataCompra,
                    PercentualCashback = item.PercentualCashback,
                    ValorCashback = item.ValorCashback,
                    Status = item.Status.Descricao,
                    CpfUsadoCompra = item.CpfRevendedor,
                    NomeRevendedor = item.Revendedor.Nome,
                    CpfRevendedor = item.Revendedor.Cpf
                }).FirstOrDefaultAsync();

        public async Task<bool> IsExist(int id)
            => await context.Compras.AnyAsync(item => item.Id.Equals(id));

        public async Task<bool> Update(Compra entity, string usuario)
        {
            Compra compra = await helperService.GetEntityAntiga<Compra>(entity.Id);
            string oldJson = JsonConvert.SerializeObject(compra);

            entity.DataCriacao = compra.DataCriacao;
            entity.DataAlteracao = DateTime.Now;

            string newJson = JsonConvert.SerializeObject(entity);

            context.Entry(entity).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();

                await historicoService.Create(new Historico
                {
                    ChaveTabela = entity.Id,
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
