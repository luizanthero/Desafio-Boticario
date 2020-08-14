using System.ComponentModel.DataAnnotations;

namespace boticario.ViewModels
{
    public class JsonHistoricoViewModel
    {
        [Required]
        public string Campo { get; set; }

        public string ValorAntes { get; set; }

        public string ValorDepois { get; set; }
    }
}
