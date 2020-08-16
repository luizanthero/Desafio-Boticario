using boticario.Business.Interfaces;
using boticario.Helpers;
using boticario.Helpers.Enums;
using boticario.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        private readonly ILogger<StatusCompraService> logger;

        private readonly string serviceName = nameof(StatusCompraService);

        public StatusCompraService(AppDbContext context, HistoricoService historicoService, HelperService helperService,
            ILogger<StatusCompraService> logger)
        {
            this.context = context;
            this.historicoService = historicoService;
            this.helperService = helperService;
            this.logger = logger;
        }

        public async Task<StatusCompra> Create(StatusCompra entity, string usuario)
        {
            const string methodName = nameof(Create);
            string header = $"METHOD | {usuario} | {serviceName}: {methodName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.InsertItem,
                    $"{header} - {MessageLog.Saving.Value}");

                context.StatusCompra.Add(entity);

                await context.SaveChangesAsync();

                logger.LogInformation((int)LogEventEnum.Events.InsertItem,
                    $"{header} - {MessageLog.Saved.Value} - ID: {entity.Id}");

                string json = JsonConvert.SerializeObject(entity);

                await historicoService.Create(new Historico
                {
                    ChaveTabela = entity.Id,
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
            const string methodName = nameof(DeleteById);
            string header = $"METHOD | {usuario} | {serviceName}: {methodName}";

            logger.LogInformation((int)LogEventEnum.Events.DeleteItem,
                $"{header} - {MessageLog.Deleting.Value}");

            StatusCompra entity = await context.StatusCompra.FindAsync(id);

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

                context.StatusCompra.Update(entity);

                await context.SaveChangesAsync();

                logger.LogInformation((int)LogEventEnum.Events.DeleteItem,
                    $"{header} - {MessageLog.Deleted.Value} - ID: {entity.Id}");

                await historicoService.Create(new Historico
                {
                    ChaveTabela = entity.Id,
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

        public async Task<IEnumerable<StatusCompra>> GetAll(string usuario)
        {
            const string methodName = nameof(GetAll);
            string header = $"METHOD | {usuario} | {serviceName}: {methodName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.GettingList.Value}");

                List<StatusCompra> result = await context.StatusCompra.Where(item => (bool)item.Ativo).ToListAsync();

                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                   $"{header} - {MessageLog.Getted.Value} - Quantidade: {result.Count()}");

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<StatusCompra> GetById(int id, string usuario)
        {
            const string methodName = nameof(GetById);
            string header = $"METHOD | {usuario} | {serviceName}: {methodName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Getting.Value}");

                StatusCompra result = await context.StatusCompra.FirstOrDefaultAsync(item => item.Id.Equals(id));

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
            => await context.StatusCompra.AnyAsync(item => item.Id.Equals(id));

        public async Task<bool> Update(StatusCompra entity, string usuario)
        {
            const string methodName = nameof(Update);
            string header = $"METHOD | {usuario} | {serviceName}: {methodName}";

            logger.LogInformation((int)LogEventEnum.Events.GetItem,
                $"{header} - {MessageLog.Getting.Value} - {MessageLog.GettingOldEntity.Value} - ID: {entity.Id} ");

            StatusCompra oldEntity = await helperService.GetEntityAntiga<StatusCompra>(entity.Id);
            string oldJson = JsonConvert.SerializeObject(oldEntity);

            logger.LogInformation((int)LogEventEnum.Events.GetItem,
                $"{header} - {MessageLog.Getted.Value} - ID: {entity.Id} ");

            entity.DataCriacao = oldEntity.DataCriacao;
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
