using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace boticario.Models
{
    [Table(name: "StatusCompra")]
    public class StatusCompra
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Descricao { get; set; }

        [JsonIgnore]
        public bool? Ativo { get; set; } = true;

        [JsonIgnore]
        public DateTime DataCriacao { get; set; }

        [JsonIgnore]
        public DateTime DataAlteracao { get; set; }
    }
}
