using System;
using System.Collections.Generic;

namespace boticario.ViewModels
{
    public class HistoricoViewModel
    {
        public int Id { get; set; }

        public string TipoHistorico { get; set; }

        public string Usuario { get; set; }

        public string NomeTabela { get; set; }

        public int ChaveTabela { get; set; }

        public List<JsonHistoricoViewModel> Json { get; set; }

        public DateTime Data { get; set; }
    }
}
