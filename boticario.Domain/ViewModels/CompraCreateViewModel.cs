using System;

namespace boticario.ViewModels
{
    public class CompraCreateViewModel
    {
        public string Codigo { get; set; }

        public double Valor { get; set; }

        public DateTime Data { get; set; }

        public string CpfRevendedor { get; set; }
    }
}
