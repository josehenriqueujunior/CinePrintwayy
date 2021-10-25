using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CinePrintwayy.CustomValidation
{
    public class CustomValidation
    {
        public class CustomValidationTipoAnimacao : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value == null)
                    return false;

                string tipoAnimacao = value.ToString();
                return (tipoAnimacao == "2D" || tipoAnimacao == "3D");
            }
        }

        public class CustomValidationTipoAudio : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value == null)
                    return false;

                string tipoAudio = value.ToString();
                return (tipoAudio == "Legendado" || tipoAudio == "Dublado");
            }
        }
    }
}
