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

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.InsertItem,
                    $"{entity.Email} | {serviceName}: {methodName} - {MessageLog.Saving.Value}");

                if (string.IsNullOrEmpty(entity.Senha))
                {
                    logger.LogWarning((int)LogEventEnum.Events.GetItemNotFound,
                        $"{entity.Email} | {serviceName}: {methodName} - {MessageError.PasswordNullorEmpty.Value}");

                    throw new Exception(MessageError.PasswordNullorEmpty.Value);
                }

                entity.Senha = HashOptions.CreatePasswordHash(entity.Senha);

                context.Revendedores.Add(entity);

                await context.SaveChangesAsync();

                logger.LogInformation((int)LogEventEnum.Events.InsertItem,
                    $"{entity.Email} | {serviceName}: {methodName} - {MessageLog.Saved.Value} | ID: {entity.Id}");

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

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{email} | {serviceName}: {methodName} - {MessageLog.Getting.Value}");

                Revendedor revendedor = await context.Revendedores.SingleOrDefaultAsync(item => item.Email.Equals(email));

                if (revendedor is null)
                {
                    logger.LogWarning((int)LogEventEnum.Events.GetItem,
                        $"{email} | {serviceName}: {methodName} - {MessageError.NotFoundSingle.Value}");

                    return string.Empty;
                }

                if (!HashOptions.VerifyPasswordHash(senha, revendedor.Senha))
                {
                    logger.LogWarning((int)LogEventEnum.Events.GetItem,
                        $"{email} | {serviceName}: {methodName} - {MessageError.NotFoundSingle.Value}");

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

                logger.LogWarning((int)LogEventEnum.Events.GetItem,
                    $"{email} | {serviceName}: {methodName} - {MessageLog.Getted.Value}");

                return tokenHandler.WriteToken(stringToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Revendedor> Create(Revendedor entity, string usuario)
        {
            try
            {
                if (string.IsNullOrEmpty(entity.Senha))
                    throw new Exception(MessageError.PasswordNullorEmpty.Value);

                entity.Senha = HashOptions.CreatePasswordHash(entity.Senha);

                context.Revendedores.Add(entity);

                await context.SaveChangesAsync();

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

        public async Task<IEnumerable<Revendedor>> GetAll()
            => await context.Revendedores.Where(item => (bool)item.Ativo).ToListAsync();

        public async Task<Revendedor> GetById(int id)
            => await context.Revendedores.FirstOrDefaultAsync(item => item.Id.Equals(id));

        public async Task<bool> IsExist(int id)
            => await context.Revendedores.AnyAsync(item => item.Id.Equals(id));

        public async Task<bool> Update(Revendedor entity, string usuario)
        {
            Revendedor revendedor = await helperService.GetEntityAntiga<Revendedor>(entity.Id);
            string oldJson = JsonConvert.SerializeObject(revendedor);


            if (!string.IsNullOrEmpty(entity.Senha))
                entity.Senha = HashOptions.CreatePasswordHash(entity.Senha);
            else
                entity.Senha = revendedor.Senha;

            entity.DataCriacao = revendedor.DataCriacao;
            entity.DataAlteracao = DateTime.Now;

            string newJson = JsonConvert.SerializeObject(entity);

            context.Entry(entity).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();

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
