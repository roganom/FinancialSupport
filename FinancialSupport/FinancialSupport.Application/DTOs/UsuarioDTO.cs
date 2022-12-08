using FinancialSupport.Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FinancialSupport.Application.DTOs
{
    public class UsuarioDTO
    {
        public int? Id { get; set; }

        [DisplayName("Nome do cliente")]
        [Required(ErrorMessage = "É obrigatório informar o nome do cliente.")]
        [MinLength(3)]
        [MaxLength(100)]
        public string? Nome { get; set; }

        [DisplayName("Nome do arquivo com a foto")]
        [MinLength(3)]
        [MaxLength(100)]
        public string? Foto { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal? Limite { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal? LimiteDisponivel { get; set; }
        public List<Emprestimo>? Emprestimos { get; set; }
    }
}
