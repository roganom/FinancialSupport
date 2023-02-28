using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FinancialSupport.Application.DTOs
{
    public class ParcelaDTO
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Informar o IdEmprestimo(parcela) obrigatóriamente.")]
        [DisplayName("Identificador")]
        public int? IdEmprestimo { get; set; }

        [Required(ErrorMessage = "Informar a data da parcela.")]
        [DisplayName("Data de vencimento")]
        [DataType(DataType.Date)]
        public DateTime DataParcela { get; set; }

        [Required(ErrorMessage = "Informar o valor da parcela.")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [DisplayName("Valor da Parcela")]
        public decimal ValorParcela { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Data de pagamento")]
        public DateTime? DataPagamento { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [DisplayName("Valor do pagamento")]
        public DateTime? DataCriacao { get; set; }
        public string? UsuarioCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string? UsuarioAlteracao { get; set; }
        public Boolean? Valendo { get; set; }
        public decimal? ValorPagamento { get; set; }
    }
}
