using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CinePrintwayy.Models
{

    [Index(nameof(Titulo), IsUnique = true)]
    public class Filme
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Imagem { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        [DataType(DataType.Time)]
        public DateTime Duracao { get; set; }
    }
}
