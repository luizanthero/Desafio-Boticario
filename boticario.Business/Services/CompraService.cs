using boticario.Business;
using boticario.Helpers;
using boticario.Helpers.Enums;
using boticario.Models;
using boticario.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        private readonly ILogger<CompraService> logger;

        private readonly string serviceName = nameof(CompraService);

        public CompraService(AppDbContext context, HistoricoService historicoService, HelperService helperService, 
            RegrasCompra regrasCompra, ILogger<CompraService> logger)
        {
            this.context = context;
            this.historicoService = historicoService;
            this.helperService = helperService;
            this.regrasCompra = regrasCompra;
            this.logger = logger;
        }

        public async Task<Compra> Create(Compra entity, string usuario)
        {
            const string methodName = nameof(Create);
            string header = $"METHOD | {usuario} | {serviceName}: {methodName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Getting.Value} - Buscando {nameof(Revendedor)} por E-Mail: {usuario}");

                Revendedor revendedor = await context.Revendedores.AsNoTracking()
                    .SingleOrDefaultAsync(item => item.Email.Equals(usuario));

                if (revendedor is null)
                {
                    logger.LogWarning((int)LogEventEnum.Events.GetItemNotFound,
                        $"{header} - {MessageError.NotFoundSingle.Value} E-Mail de busca: {usuario}");

                    return null;
                }

                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Getted.Value} - ID Revendedor: {revendedor.Id}");

                entity.IdRevendedor = revendedor.Id;

                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Getting.Value} - Buscando Status pelo CPF: {entity.CpfRevendedor}");

                entity.IdStatus = await regrasCompra.GetStatusCompraId(entity.CpfRevendedor, usuario);

                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Getted.Value} - ID Status: {entity.Status}");

                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Getting.Value} - Buscando Regra de Percentual de Cashback pelo Valor: {entity.Valor}");

                entity.PercentualCashback = await regrasCompra.GetPercentualCashback(entity.Valor, usuario);

                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Getted.Value} - Percentual Cashback: {entity.PercentualCashback}");

                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Getting.Value} - Realizando calculo do Valor de Cashback ({entity.PercentualCashback} / 100) * {entity.Valor}");

                entity.ValorCashback = (entity.PercentualCashback / 100) * entity.Valor;

                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Getted.Value} - Valor Cashback: {entity.ValorCashback}");

                logger.LogInformation((int)LogEventEnum.Events.InsertItem,
                    $"{header} - {MessageLog.Saving.Value}");

                context.Compras.Add(entity);

                await context.SaveChangesAsync();

                logger.LogInformation((int)LogEventEnum.Events.InsertItem,
                    $"{header} - {MessageLog.Saved.Value} - ID: {entity.Id}");

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
            const string methodName = nameof(DeleteById);
            string header = $"METHOD | {usuario} | {serviceName}: {methodName}";

            logger.LogInformation((int)LogEventEnum.Events.DeleteItem,
                $"{header} - {MessageLog.Deleting.Value}");

            Compra entity = await context.Compras.FindAsync(id);

            if (entity is null)
            {
                logger.LogWarning((int)LogEventEnum.Events.DeleteItemNotFound,
                    $"{header} - {MessageLog.DeleteNotFound.Value} - ID: {id}");

                return false;
            }

            string json = JsonConvert.SerializeObject(entity);

            try
            {
                entity.Ativo = false;

                context.Compras.Update(entity);

                await context.SaveChangesAsync();

                logger.LogInformation((int)LogEventEnum.Events.DeleteItem,
                    $"{header} - {MessageLog.Deleted.Value} - ID: {entity.Id}");

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

        public async Task<IEnumerable<CompraViewModel>> GetAll(string usuario)
        {
            const string methodName = nameof(GetAll);
            string header = $"METHOD | {usuario} | {serviceName}: {methodName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.GettingList.Value}");

                List<CompraViewModel> result = await context.Compras
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

                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Getted.Value} - Quantidade: {result.Count()}");

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CompraViewModel> GetById(int id, string usuario)
        {
            const string methodName = nameof(GetById);
            string header = $"METHOD | {usuario} | {serviceName}: {methodName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.GettingList.Value}");

                CompraViewModel result = await context.Compras
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

                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Getted.Value} - ID: {id}");

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsExist(int id)
            => await context.Compras.AnyAsync(item => item.Id.Equals(id));

        public async Task<bool> Update(Compra entity, string usuario)
        {
            const string methodName = nameof(Update);
            string header = $"METHOD | {usuario} | {serviceName}: {methodName}";

            logger.LogInformation((int)LogEventEnum.Events.GetItem,
                $"{header} - {MessageLog.Getting.Value} - {MessageLog.GettingOldEntity.Value} - ID: {entity.Id} ");

            Compra compra = await helperService.GetEntityAntiga<Compra>(entity.Id);
            string oldJson = JsonConvert.SerializeObject(compra);

            logger.LogInformation((int)LogEventEnum.Events.GetItem,
                $"{header} - {MessageLog.Getted.Value} - ID: {entity.Id} ");

            entity.DataCriacao = compra.DataCriacao;
            entity.DataAlteracao = DateTime.Now;

            string newJson = JsonConvert.SerializeObject(entity);

            logger.LogInformation((int)LogEventEnum.Events.UpdateItem,
                $"{header} - {MessageLog.Updating.Value} - ID: {entity.Id}");

            context.Entry(entity).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();

                logger.LogInformation((int)LogEventEnum.Events.UpdateItem,
                    $"{header} - {MessageLog.Updated.Value}");

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
