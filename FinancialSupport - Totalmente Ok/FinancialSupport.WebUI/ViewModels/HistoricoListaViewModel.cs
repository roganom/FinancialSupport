using System.ComponentModel.DataAnnotations;

namespace FinancialSupport.WebUI.ViewModels
{
    public class HistoricoListaViewModel : ParcelasViewModel
    {
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public string EmprestimoInativoIdValorDasParcelas { get; set; }
        public string EmprestimoInativoIdNumeroDeParcelas { get; set; }
        public string EmprestimoInativoIdNumeroDeParcelasEmAtraso { get; set; }
    }
}
