using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CinePrintwayy.ViewModels
{
    public class FilmeViewModel : EditImageViewModel
    {
        [Required]
        [Display(Name = "Título")]
        public string Titulo { get; set; }
        [Required]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        [Display(Name = "Duração")]
        [Required]
        [DataType(DataType.Time)]  
        public DateTime Duracao { get; set; }

        [Display(Name = "Imagem")]
        [Required]
        public IFormFile Imagem { get; set; }
    }
}
