using FinancialSupport.Application.DTOs;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FinancialSupport.WebUI.ViewModels
{
    public class ListaAdmEmprestimoViewModel
    {
        [DisplayName("Id do empréstimo")]
        public int? Id { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o Id do usuário.")]
        [DisplayName("Id do cliente")]
        public int? IdUsuario { get; set; }

        [DisplayName("Nome do cliente")]
        public string NomeUsuario { get; set; }

        [Required(ErrorMessage = "Defina o valor do emprestimo.")]
        [Range(0, 9999999999999999.99)]
        [DisplayName("Valor do empréstimo")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Informar a data de contratação do empréstimo.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [DisplayName("Data de aquisição")]
        public DateTime Data { get; set; }
        public bool Ativo { get; set; }
        public int? NumeroParcelas { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string? UsuarioCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string? UsuarioAlteracao { get; set; }
        public Boolean? Valendo { get; set; }
        public List<ParcelaDTO>? Parcelas { get; set; }
    }
}
