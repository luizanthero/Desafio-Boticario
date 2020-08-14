using boticario.ExternalAPIs.boticario;
using boticario.Models;
using System;
using System.Threading.Tasks;

namespace boticario.Services
{
    public class CashbackService
    {
        public async Task<Cashback> GetCashbackPoints(string cpf)
        {
            try
            {
                Cashback result = await BoticarioConnection.Connect<Cashback>($"?cpf={cpf}");

                return result;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
