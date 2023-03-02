using System.ComponentModel.DataAnnotations;

namespace FinancialSupport.WebUI.ViewModels
{
    public class EmprestimoListaViewModel
    {
        public string Data { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public string Valor { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public string ValorParcela { get; set; }
        public string QtdeParcelasPagas { get; set; }
        public string QtdeParcelasAtrasadas { get; set; }
    }
}
