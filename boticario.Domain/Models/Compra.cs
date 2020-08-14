using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace boticario.Models
{
    [Table(name: "Compra")]
    public class Compra
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Id")]
        public int IdRevendedor { get; set; }
        public virtual Revendedor Revendedor { get; set; }

        [ForeignKey("Id")]
        [JsonIgnore]
        public int IdStatus { get; set; }
        public virtual StatusCompra Status { get; set; }

        [Required]
        public string Codigo { get; set; }

        [Required]
        public double Valor { get; set; }

        [Required]
        public string CpfRevendedor { get; set; }

        public DateTime DataCompra { get; set; }

        public double PercentualCashback { get; set; }

        public double ValorCashback { get; set; }

        public bool? Ativo { get; set; } = true;

        [JsonIgnore]
        public DateTime DataCriacao { get; set; }

        [JsonIgnore]
        public DateTime DataAlteracao { get; set; }
    }
}
