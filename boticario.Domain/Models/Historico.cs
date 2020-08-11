using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace boticario.Models
{
    [Table(name: "Historico")]
    public class Historico
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Id")]
        public int IdTipoHistorico { get; set; }
        public virtual TipoHistorico TipoHistorico { get; set; }

        [Required]
        public string Usuario { get; set; }

        [Required]
        public string NomeTabela { get; set; }

        [Required]
        public int ChaveTable { get; set; }

        public string JsonAntes { get; set; }

        public string JsonDepois { get; set; }

        [Required]
        public DateTime Data { get; set; } = DateTime.Now;
    }
}
