using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static CinePrintwayy.CustomValidation.CustomValidation;

namespace CinePrintwayy.Models
{
    public class Sessao
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }
        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Início")]
        public DateTime HorarioInicio { get; set; }
        [Required]
        [Display(Name = "Fim")]
        [DataType(DataType.Time)]
        public DateTime HorarioFim { get; set; }
        [Required]
        [Display(Name = "Preço")]
        public decimal ValorIngresso { get; set; }
        [Required]
        [Display(Name = "Animação")]
        [CustomValidationTipoAnimacao(ErrorMessage ="Animação só aceita valores [2D] e [3D]")]
        public string TipoAnimacao { get; set; }
        [Required]
        [Display(Name = "Áudio")]
        [CustomValidationTipoAudio(ErrorMessage = "Áudio só aceita valores [Legendado] e [Dublado]")]
        public string TipoAudio { get; set; }
        [Display(Name = "Filme")]
        [Required]
        public int IdFilme { get; set; }
        [NotMapped]
        public Filme Filme { get; set; }
        [Display(Name = "Sala")]
        [Required]
        public int IdSala { get; set; }
        [NotMapped]
        public Sala Sala { get; set; }
    }
}
