using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CinePrintwayy.Models
{
    public class Sala
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nome{ get; set; }
        [Required]
        [Display(Name = "Assentos")]
        public int QuantidadeAcentos { get; set; }
    }
}
