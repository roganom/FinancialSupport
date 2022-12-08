using FinancialSupport.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace FinancialSupport.WebUI.ViewModels
{
    public class DetalhesClienteViewModel
    {
        public int UsuarioId { get; set; }
        public int EmprestimoIdUsuario { get; set; }
        public int EmprestimoId { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double ParcelaIdEmprestimo { get; set; }
        public string? Foto { get; set; }
        public string? Nome { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal? Limite { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal? LimiteDisponivel { get; set; }
        public List<EmprestimoListaViewModel> EmprestimosAtivos { get; set; }
        public List<HistoricoListaViewModel> EmprestimosHistoricos { get; set; }
        public List<Parcela>? Parcelas { get; set; }

        public DetalhesClienteViewModel()
        {
            EmprestimosAtivos = new List<EmprestimoListaViewModel>();
            EmprestimosHistoricos = new List<HistoricoListaViewModel>();
        }
    }
}
