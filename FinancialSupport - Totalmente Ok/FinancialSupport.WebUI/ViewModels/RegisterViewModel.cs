using FinancialSupport.WebUI.ViewModels.Shared;
using System.ComponentModel.DataAnnotations;

namespace FinancialSupport.WebUI.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Tipo { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display (Name = "Confirme a senha")]
        [Compare("Password", ErrorMessage = "As senhas são diferentes")]
        public string ConfirmPassword { get; set;}
        public CustomMessagePartialViewModel? CustomMessagePartial { get; set; }
        public RegisterViewModel()
        {
            CustomMessagePartial = new CustomMessagePartialViewModel();
        }
    }
}
