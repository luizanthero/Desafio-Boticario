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

        [Required]
        [ForeignKey("Id")]
        public int IdStatus { get; set; }
        public virtual StatusCompra Status { get; set; }

        public string Codigo { get; set; }

        public int Valor { get; set; }

        public DateTime DataCompra { get; set; }

        public int PercentualCashback { get; set; }

        public int ValorCashback { get; set; }

        public bool Ativo { get; set; } = true;

        [JsonIgnore]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [JsonIgnore]
        public DateTime DataAlteracao { get; set; }
    }
}
