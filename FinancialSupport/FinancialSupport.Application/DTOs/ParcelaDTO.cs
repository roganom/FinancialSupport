using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FinancialSupport.Application.DTOs
{
    public class ParcelaDTO
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Informar o IdEmprestimo(parcela) obrigatóriamente.")]
        [DisplayName("Identificador do empréstimo")]
        public int? IdEmprestimo { get; set; }

        [Required(ErrorMessage = "Informar a data da parcela.")]
        [DisplayName("Data de vencimento da parcela")]
        [DataType(DataType.Date)]
        public DateTime DataParcela { get; set; }

        [Required(ErrorMessage = "Informar o valor da parcela.")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal ValorParcela { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("data de pagamento da parcela")]
        public DateTime? DataPagamento { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal? ValorPagamento { get; set; }
    }
}
