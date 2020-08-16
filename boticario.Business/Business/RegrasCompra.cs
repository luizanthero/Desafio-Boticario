using boticario.Helpers.Enums;
using boticario.Models;
using boticario.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace boticario.Business
{
    public class RegrasCompra
    {
        private readonly ParametroSistemaService parametroService;
        private readonly RegraCashbackService regraService;

        public RegrasCompra(ParametroSistemaService parametroService, RegraCashbackService regraService)
        {
            this.parametroService = parametroService;
            this.regraService = regraService;
        }

        public async Task<int> GetStatusCompraId(string cpf, string usuario)
        {
            try
            {
                string cpfCoringa = (await parametroService.GetById((int)ParametroSistemaEnum.Parameter.CpfCoringa, usuario)).Valor;

                if (cpf.Equals(cpfCoringa))
                    return (int)StatusCompraEnum.Status.Aprovado;
                else
                    return (int)StatusCompraEnum.Status.EmValidacao;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> GetPercentualCashback(double valorCompra, string usuario)
        {
            try
            {
                List<RegraCashback> regras = (await regraService.GetAll(usuario)).ToList();

                int percentual = 0;

                foreach (var item in regras)
                {
                    if (!item.Fim.Equals(0))
                    {
                        if (valorCompra >= item.Inicio && valorCompra < item.Fim)
                            percentual = item.Percentual;
                    }
                    else
                    {
                        if (valorCompra >= item.Inicio)
                            percentual = item.Percentual;
                    }

                }

                return percentual;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
