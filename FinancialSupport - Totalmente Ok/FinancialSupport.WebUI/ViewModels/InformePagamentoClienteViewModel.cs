using FinancialSupport.Domain.Entities;
using FinancialSupport.WebUI.ViewModels.Shared;
using System.ComponentModel.DataAnnotations;

namespace FinancialSupport.WebUI.ViewModels
{
    public class InformePagamentoClienteViewModel
    {
        public int? IdUsuario { get; set; }
        public int? IdEmprestimo { get; set; }
        public string? Foto { get; set; }
        public string? Nome { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal? Limite { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal? LimiteDisponivel { get; set; }
        public string Operador { get; set; }
        public List<EmprestimoListaViewModel> EmprestimosAtivos { get; set; }
        public List<ParcelasViewModel> PagamentoParcelas { get; set; }
        public List<Parcela>? Parcelas { get; set; }
        public CustomMessagePartialViewModel? CustomMessagePartial { get; set; }
        public InformePagamentoClienteViewModel()
        {
            EmprestimosAtivos = new List<EmprestimoListaViewModel>();
            PagamentoParcelas = new List<ParcelasViewModel>();
            CustomMessagePartial = new CustomMessagePartialViewModel();
        }
    }
}
