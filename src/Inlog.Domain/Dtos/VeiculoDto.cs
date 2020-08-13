
using Inlog.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace Inlog.Domain.Dtos
{
    public class VeiculoDto
    {

       
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Chassi { get; set; }

        
        [Display(Name = "Tipo do veiculo")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public TipoVeiculo TipoVeiculo { get; set; }
     

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Cor { get; set; }
    }
}
