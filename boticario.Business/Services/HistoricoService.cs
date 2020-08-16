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
    public class HistoricoService
    {
        private readonly AppDbContext context;
        private readonly ILogger<HistoricoService> logger;

        private readonly string serviceName = nameof(HistoricoService);

        public HistoricoService(AppDbContext context, ILogger<HistoricoService> logger)
        {
            this.context = context;
            this.logger = logger;
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

        public async Task<IEnumerable<HistoricoViewModel>> GetAll(string usuario)
        {
            const string methodName = nameof(GetAll);
            string header = $"METHOD | {usuario} | {serviceName}: {methodName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.GettingList.Value}");

                var result = await context.Historicos.Select(item => new HistoricoViewModel
                {
                    Id = item.Id,
                    TipoHistorico = context.TiposHistorico.FirstOrDefault(tipo => tipo.Id.Equals(item.IdTipoHistorico)).Descricao,
                    NomeTabela = item.NomeTabela,
                    ChaveTabela = item.ChaveTabela,
                    Usuario = item.Usuario,
                    Json = GetJsonHistorico(item),
                    Data = item.Data
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

        public async Task<HistoricoViewModel> GetById(int id, string usuario)
        {
            const string methodName = nameof(GetById);
            string header = $"METHOD | {usuario} | {serviceName}: {methodName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.GettingList.Value}");

                HistoricoViewModel result = await context.Historicos.Select(item => new HistoricoViewModel
                {
                    Id = item.Id,
                    TipoHistorico = context.TiposHistorico.FirstOrDefault(tipo => tipo.Id.Equals(item.IdTipoHistorico)).Descricao,
                    NomeTabela = item.NomeTabela,
                    ChaveTabela = item.ChaveTabela,
                    Usuario = item.Usuario,
                    Json = GetJsonHistorico(item),
                    Data = item.Data
                })
                    .FirstOrDefaultAsync(item => item.Id.Equals(id));

                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Getted.Value} - ID: {id}");

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<HistoricoViewModel>> GetByNomeTabela(string nomeTabela, string usuario)
        {
            const string methodName = nameof(GetByNomeTabela);
            string header = $"METHOD | {usuario} | {serviceName}: {methodName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.GettingList.Value}");

                List<HistoricoViewModel> result = await context.Historicos.Select(item => new HistoricoViewModel
                {
                    Id = item.Id,
                    TipoHistorico = context.TiposHistorico.FirstOrDefault(tipo => tipo.Id.Equals(item.IdTipoHistorico)).Descricao,
                    NomeTabela = item.NomeTabela,
                    ChaveTabela = item.ChaveTabela,
                    Usuario = item.Usuario,
                    Json = GetJsonHistorico(item),
                    Data = item.Data
                })
                    .Where(item => item.NomeTabela.Equals(nomeTabela)).ToListAsync();

                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Getted.Value} - Nome da Tabela: {nomeTabela}");

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<HistoricoViewModel>> GetByChaveTabela(int chaveTabela, string usuario)
        {
            const string methodName = nameof(GetByChaveTabela);
            string header = $"METHOD | {usuario} | {serviceName}: {methodName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.GettingList.Value}");

                List<HistoricoViewModel> result = await context.Historicos.Select(item => new HistoricoViewModel
                {
                    Id = item.Id,
                    TipoHistorico = context.TiposHistorico.FirstOrDefault(tipo => tipo.Id.Equals(item.IdTipoHistorico)).Descricao,
                    NomeTabela = item.NomeTabela,
                    ChaveTabela = item.ChaveTabela,
                    Usuario = item.Usuario,
                    Json = GetJsonHistorico(item),
                    Data = item.Data
                })
                    .Where(item => item.ChaveTabela.Equals(chaveTabela)).ToListAsync();

                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Getted.Value} - Chave da Tabela: {chaveTabela}");

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<HistoricoViewModel>> GetByTabelaChave(string nomeTabela, int chaveTabela, string usuario)
        {
            const string methodName = nameof(GetByTabelaChave);
            string header = $"METHOD | {usuario} | {serviceName}: {methodName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.GettingList.Value}");

                List<HistoricoViewModel> result = await context.Historicos.Select(item => new HistoricoViewModel
                {
                    Id = item.Id,
                    TipoHistorico = context.TiposHistorico.FirstOrDefault(tipo => tipo.Id.Equals(item.IdTipoHistorico)).Descricao,
                    NomeTabela = item.NomeTabela,
                    ChaveTabela = item.ChaveTabela,
                    Usuario = item.Usuario,
                    Json = GetJsonHistorico(item),
                    Data = item.Data
                })
                    .Where(item => item.NomeTabela.Equals(nomeTabela) && item.ChaveTabela.Equals(chaveTabela)).ToListAsync();

                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Getted.Value} - Chave da Tabela: {chaveTabela}");

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

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

        private static List<JsonHistoricoViewModel> GetJsonHistorico(Historico historico)
        {
            List<JsonHistoricoViewModel> result = new List<JsonHistoricoViewModel>();

            if (string.IsNullOrEmpty(historico.JsonDepois))
            {
                IDictionary<string, object> entity = JsonConvert.DeserializeObject<Dictionary<string, object>>(historico.JsonAntes);

                foreach (KeyValuePair<string, object> item in entity)
                {
                    result.Add(new JsonHistoricoViewModel
                    {
                        Campo = item.Key,
                        ValorAntes = (item.Value is null) ? string.Empty : item.Value.ToString(),
                        ValorDepois = string.Empty
                    });
                }
            }

            if (string.IsNullOrEmpty(historico.JsonAntes))
            {
                IDictionary<string, object> entity = JsonConvert.DeserializeObject<Dictionary<string, object>>(historico.JsonDepois);

                foreach (KeyValuePair<string, object> item in entity)
                {
                    result.Add(new JsonHistoricoViewModel
                    {
                        Campo = item.Key,
                        ValorAntes = string.Empty,
                        ValorDepois = (item.Value is null) ? string.Empty : item.Value.ToString()
                    });
                }
            }

            if (!string.IsNullOrEmpty(historico.JsonAntes) && !string.IsNullOrEmpty(historico.JsonDepois))
            {
                IDictionary<string, object> oldEntity = JsonConvert.DeserializeObject<Dictionary<string, object>>(historico.JsonAntes);
                IDictionary<string, object> newEntity = JsonConvert.DeserializeObject<Dictionary<string, object>>(historico.JsonDepois);

                foreach (KeyValuePair<string, object> oldItem in oldEntity)
                {
                    foreach (KeyValuePair<string, object> newItem in newEntity)
                    {
                        if (newItem.Key.Equals(oldItem.Key))
                        {
                            result.Add(new JsonHistoricoViewModel
                            {
                                Campo = newItem.Key,
                                ValorAntes = (oldItem.Value is null) ? string.Empty : oldItem.Value.ToString(),
                                ValorDepois = (newItem.Value is null) ? string.Empty : newItem.Value.ToString()
                            });

                            break;
                        }
                    }
                }
            }

            return result;
        }
    }
}
