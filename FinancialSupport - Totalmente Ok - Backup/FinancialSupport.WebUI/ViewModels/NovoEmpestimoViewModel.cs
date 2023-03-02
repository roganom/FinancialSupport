using FinancialSupport.WebUI.ViewModels.Shared;
using System.ComponentModel.DataAnnotations;

namespace FinancialSupport.WebUI.ViewModels
{
    public class NovoEmpestimoViewModel
    {
        [Required]
        public int? IdUsuario { get; set; }
        public string? Nome { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal Valor { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal LimiteDisponivel { get; set; }   
        public int NumeroParcelas { get; set; }
        public int Juros { get; set; }
        public string Operador { get; set; } //  UsuarioCriacao ou UsuarioAlteracao
        public CustomMessagePartialViewModel? CustomMessagePartial { get; set; }
        public NovoEmpestimoViewModel()
        {
            CustomMessagePartial = new CustomMessagePartialViewModel();
        }
    }
}
