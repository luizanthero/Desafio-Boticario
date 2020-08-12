﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace boticario.Models
{
    [Table(name: "TipoHistorico")]
    public class TipoHistorico
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Descricao { get; set; }

        [JsonIgnore]
        public bool Ativo { get; set; } = true;

        [JsonIgnore]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [JsonIgnore]
        public DateTime DataAlteracao { get; set; }
    }
}
