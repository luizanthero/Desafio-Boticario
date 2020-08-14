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

        public async Task<IEnumerable<HistoricoViewModel>> GetAll()
            => await context.Historicos.Select(item => new HistoricoViewModel
            {
                Id = item.Id,
                TipoHistorico = context.TiposHistorico.FirstOrDefault(tipo => tipo.Id.Equals(item.IdTipoHistorico)).Descricao,
                NomeTabela = item.NomeTabela,
                ChaveTabela = item.ChaveTabela,
                Usuario = item.Usuario,
                Json = GetJsonHistorico(item),
                Data = item.Data
            }).ToListAsync();

        public async Task<HistoricoViewModel> GetById(int id)
            => await context.Historicos.Select(item => new HistoricoViewModel
            {
                Id = item.Id,
                TipoHistorico = context.TiposHistorico.FirstOrDefault(tipo => tipo.Id.Equals(item.IdTipoHistorico)).Descricao,
                NomeTabela = item.NomeTabela,
                ChaveTabela = item.ChaveTabela,
                Usuario = item.Usuario,
                Json = GetJsonHistorico(item),
                Data = item.Data
            }).FirstOrDefaultAsync(item => item.Id.Equals(id));

        public async Task<IEnumerable<HistoricoViewModel>> GetByNomeTabela(string nomeTabela)
            => await context.Historicos.Select(item => new HistoricoViewModel
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

        public async Task<IEnumerable<HistoricoViewModel>> GetByChaveTabela(int chaveTabela)
            => await context.Historicos.Select(item => new HistoricoViewModel
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

        public async Task<IEnumerable<HistoricoViewModel>> GetByTabelaChave(string nomeTabela, int chaveTabela)
            => await context.Historicos.Select(item => new HistoricoViewModel
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
