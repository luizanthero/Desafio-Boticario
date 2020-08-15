using boticario.ExternalAPIs.boticario;
using boticario.Helpers.Enums;
using boticario.Models;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Threading.Tasks;

namespace boticario.Services
{
    public class CashbackService
    {
        private readonly ILogger<CashbackService> logger;

        private readonly string serviceName = nameof(CashbackService);

        public CashbackService(ILogger<CashbackService> logger)
        {
            this.logger = logger;
        }

        public async Task<Cashback> GetCashbackPoints(string cpf, string usuario)
        {
            const string methodName = nameof(GetCashbackPoints);
            string header = $"METHOD | {usuario} | {serviceName}: {methodName}";

            try
            {
                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Getting.Value}");

                Cashback result = await BoticarioConnection.Connect<Cashback>($"?cpf={cpf}");

                logger.LogInformation((int)LogEventEnum.Events.GetItem,
                    $"{header} - {MessageLog.Getted.Value} - Credit: {result.Body.Credit}");

                return result;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
