using System.ComponentModel.DataAnnotations;

namespace FinancialSupport.API.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "É obrigatório informar o e-mail")]
        [EmailAddress(ErrorMessage = "Formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "É obrigatório informar a senha")]
        [StringLength(20, ErrorMessage = "A {0} deve ter no mínimo {2} e no máximo {1} caracteres.", MinimumLength = 10)]
        [DataType(DataType.Password)]

        public string Password { get; set; }
    }
}
