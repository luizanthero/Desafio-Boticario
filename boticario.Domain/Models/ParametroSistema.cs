using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace boticario.Models
{
    [Table(name: "ParametroSistema")]
    public class ParametroSistema
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NomeParametro { get; set; }

        [Required]
        public string Valor { get; set; }

        [JsonIgnore]
        public bool Ativo { get; set; } = true;

        [JsonIgnore]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [JsonIgnore]
        public DateTime DataAlteracao { get; set; }
    }
}
