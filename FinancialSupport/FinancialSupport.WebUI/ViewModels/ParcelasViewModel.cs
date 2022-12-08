namespace FinancialSupport.WebUI.ViewModels
{
    public class ParcelasViewModel
    {
        public int? IdParcela { get; set; }
        public int? IdEmprestimo { get; set; }
        public string? ValorEmprestimo { get; set; }
        public string? EmprestimoInativoIdData { get; set; }
        public string? EmprestimoInativoIdValor { get; set; }
        public decimal? ValorPago { get; set; }
    }
}
