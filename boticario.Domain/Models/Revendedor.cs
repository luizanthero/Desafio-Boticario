using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace boticario.Models
{
    [Table(name: "Revendedor")]
    public class Revendedor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Cpf { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }

        public bool Ativo { get; set; } = true;

        [JsonIgnore]
        public DateTime Criacao { get; set; } = DateTime.Now;

        [JsonIgnore]
        public DateTime Alteracao { get; set; }
    }
}
