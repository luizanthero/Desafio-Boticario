using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace boticario.Models
{
    [Table(name: "RegraCashback")]
    public class RegraCashback
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Inicio { get; set; }

        public int Fim { get; set; }

        [Required]
        public int Percentual { get; set; }

        [JsonIgnore]
        public bool Ativo { get; set; } = true;

        [JsonIgnore]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [JsonIgnore]
        public DateTime DataAlteracao { get; set; }
    }
}
