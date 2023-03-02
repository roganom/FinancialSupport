using System.ComponentModel.DataAnnotations;

namespace FinancialSupport.WebUI.ViewModels
{
    public class ParcelasViewModel
    {
        public int? IdParcela { get; set; }
        public int? IdEmprestimo { get; set; }
        public string? ValorEmprestimo { get; set; }
        public string? EmprestimoInativoIdData { get; set; }
        public string? EmprestimoInativoIdValor { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal? ValorPago { get; set; }
    }
}
