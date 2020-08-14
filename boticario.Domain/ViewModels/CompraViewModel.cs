using System;

namespace boticario.ViewModels
{
    public class CompraViewModel
    {
        public string Codigo { get; set; }

        public double Valor { get; set; }

        public DateTime Data { get; set; }

        public double PercentualCashback { get; set; }

        public double ValorCashback { get; set; }

        public string Status { get; set; }

        public string NomeRevendedor { get; set; }

        public string CpfRevendedor { get; set; }

        public string CpfUsadoCompra { get; set; }
    }
}
