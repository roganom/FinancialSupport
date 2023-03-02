using FinancialSupport.WebUI.ViewModels.Shared;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FinancialSupport.WebUI.ViewModels
{
    public class TrocaSenhaViewModel
    {
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Entre com a senha antiga")]
        public string OldPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Entre com a senha nova")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display (Name = "Confirme a senha")]
        [Compare("NewPassword", ErrorMessage = "As senhas novas são diferentes")]
        [DisplayName("Confirme a senha nova")]
        public string ConfirmNewPassword { get; set;}
        public CustomMessagePartialViewModel? CustomMessagePartial { get; set; }
        public TrocaSenhaViewModel()
        {
            CustomMessagePartial = new CustomMessagePartialViewModel();
        }
    }
}
