using boticario.Business.Interfaces;
using boticario.Helpers;
using boticario.Helpers.Enums;
using boticario.Helpers.Security;
using boticario.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace boticario.Services
{
    public class RevendedorService : IRepository<Revendedor>
    {
        private readonly AppDbContext context;
        private readonly HistoricoService historicoService;
        private readonly HelperService helperService;

        private readonly SettingsOptions settingsOptions;
        private readonly ILogger<RevendedorService> logger;

        private readonly string serviceName = nameof(RevendedorService);

        public RevendedorService(AppDbContext context, HistoricoService historicoService, HelperService helperService, 
            IOptions<SettingsOptions> settingsOptions, ILogger<RevendedorService> logger)
        {
            this.context = context;
            this.historicoService = historicoService;
            this.helperService = helperService;
            this.settingsOptions = settingsOptions.Value;
            this.logger = logger;
        }

        public async Task<Revendedor> Register(Revendedor entity)
        {
            const string methodName = nameof(Register);
            string header = $"METHOD | {entity.Email} | {serviceName}: {methodName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.InsertItem,
                    $"{header} - {MessageLog.Saving.Value}");

                if (string.IsNullOrEmpty(entity.Senha))
                {
                    logger.LogWarning((int)LogEventEnum.Events.GetItemNotFound,
                        $"{header} - {MessageError.PasswordNullorEmpty.Value}");

                    throw new Exception(MessageError.PasswordNullorEmpty.Value);
                }

                entity.Senha = HashOptions.CreatePasswordHash(entity.Senha);

                entity.Cpf = entity.Cpf.Replace(".", string.Empty).Replace("-", string.Empty)
                    .Replace(";", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty);

                context.Revendedores.Add(entity);

                await context.SaveChangesAsync();

                logger.LogInformation((int)LogEventEnum.Events.InsertItem,
                    $"{header} - {MessageLog.Saved.Value} | ID: {entity.Id}");

                string json = JsonConvert.SerializeObject(entity);

                await historicoService.Create(new Historico
                {
                    ChaveTabela = entity.Id,
                    NomeTabela = typeof(Revendedor).Name,
                    JsonAntes = string.Empty,
                    JsonDepois = json,
                    Usuario = entity.Email,
                    IdTipoHistorico = (int)TipoHistoricoEnum.Action.Register
                });

                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> Authentication(string email, string senha)
        {
            const string methodName = nameof(Authentication);
            string header = $"METHOD | {email} | {serviceName}: {methodName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Getting.Value}");

                Revendedor revendedor = await context.Revendedores.SingleOrDefaultAsync(item => item.Email.Equals(email));

                if (revendedor is null)
                {
                    logger.LogWarning((int)LogEventEnum.Events.GetItem,
                        $"{header} - {MessageError.NotFoundSingle.Value}");

                    return string.Empty;
                }

                if (!HashOptions.VerifyPasswordHash(senha, revendedor.Senha))
                {
                    logger.LogWarning((int)LogEventEnum.Events.GetItem,
                        $"{header} - {MessageError.NotFoundSingle.Value}");

                    return string.Empty;
                }

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, revendedor.Email));

                claims.Add(new Claim(ClaimTypes.Role, "all"));

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                byte[] key = Encoding.ASCII.GetBytes(settingsOptions.Secret);
                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                SecurityToken stringToken = tokenHandler.CreateToken(tokenDescriptor);

                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Getted.Value}");

                return tokenHandler.WriteToken(stringToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Revendedor> Create(Revendedor entity, string usuario)
        {
            const string methodName = nameof(Create);
            string header = $"METHOD | {usuario} | {serviceName}: {methodName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.InsertItem,
                    $"{header} - {MessageLog.Saving.Value}");

                entity.Cpf = entity.Cpf.Replace(".", string.Empty).Replace("-", string.Empty)
                    .Replace(";", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty);

                context.Revendedores.Add(entity);

                await context.SaveChangesAsync();

                logger.LogInformation((int)LogEventEnum.Events.InsertItem,
                    $"{header} - {MessageLog.Saved.Value} - ID: {entity.Id}");

                string json = JsonConvert.SerializeObject(entity);

                await historicoService.Create(new Historico
                {
                    ChaveTabela = entity.Id,
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
            const string methodName = nameof(DeleteById);
            string header = $"METHOD | {usuario} | {serviceName}: {methodName}";

            logger.LogInformation((int)LogEventEnum.Events.DeleteItem,
                $"{header} - {MessageLog.Deleting.Value}");

            Revendedor entity = await context.Revendedores.FindAsync(id);

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

                context.Revendedores.Update(entity);

                await context.SaveChangesAsync();

                logger.LogInformation((int)LogEventEnum.Events.DeleteItem,
                    $"{header} - {MessageLog.Deleted.Value} - ID: {entity.Id}");

                await historicoService.Create(new Historico
                {
                    ChaveTabela = entity.Id,
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

        public async Task<IEnumerable<Revendedor>> GetAll(string usuario)
        {
            const string methodName = nameof(GetAll);
            string header = $"METHOD | {usuario} | {serviceName}: {methodName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.GettingList.Value}");

                List<Revendedor> result = await context.Revendedores.Where(item => (bool)item.Ativo).ToListAsync();

                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Getted.Value} - Quantidade: {result.Count()}");

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Revendedor> GetById(int id, string usuario)
        {
            const string methodName = nameof(GetById);
            string header = $"METHOD | {usuario} | {serviceName}: {methodName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Getting.Value}");

                Revendedor result = await context.Revendedores.FirstOrDefaultAsync(item => item.Id.Equals(id));

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
            => await context.Revendedores.AnyAsync(item => item.Id.Equals(id));

        public async Task<bool> Update(Revendedor entity, string usuario)
        {
            const string methodName = nameof(Update);
            string header = $"METHOD | {usuario} | {serviceName}: {methodName}";

            logger.LogInformation((int)LogEventEnum.Events.GetItem,
                $"{header} - {MessageLog.Getting.Value} - {MessageLog.GettingOldEntity.Value} - ID: {entity.Id} ");

            Revendedor oldEntity = await helperService.GetEntityAntiga<Revendedor>(entity.Id);
            string oldJson = JsonConvert.SerializeObject(oldEntity);

            logger.LogInformation((int)LogEventEnum.Events.GetItem,
                $"{header} - {MessageLog.Getted.Value} - ID: {entity.Id} ");

            entity.DataCriacao = oldEntity.DataCriacao;
            entity.DataAlteracao = DateTime.Now;
            entity.Cpf = entity.Cpf.Replace(".", string.Empty).Replace("-", string.Empty)
                    .Replace(";", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty);

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
