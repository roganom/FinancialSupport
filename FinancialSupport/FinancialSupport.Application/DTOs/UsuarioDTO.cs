using FinancialSupport.Domain.Entities;
using Microsoft.AspNetCore.Http;
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
        [Required(ErrorMessage = "É obrigatório informar o limite.")]
        [DisplayName("Limite total")]
        public decimal? Limite { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [DisplayName("Limite disponível")]
        public decimal? LimiteDisponivel { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [DisplayName("Valor em aberto")]
        public DateTime? DataCriacao { get; set; }
        public string? UsuarioCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string? UsuarioAlteracao { get; set; }
        public Boolean? Valendo { get; set; }
        public List<Emprestimo>? Emprestimos { get; set; }
        public IFormFile? arquivo { get; set; }
    }
}
