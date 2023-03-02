using FinancialSupport.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace FinancialSupport.WebUI.ViewModels
{
    public class ResumoMercadoViewModel
    {
        public int UsuarioId { get; set; }
        public int EmprestimoIdUsuario { get; set; }
        public int EmprestimoId { get; set; }
        public int ParcelaIdEmprestimo { get; set; }
        public int EmprestimoAtivoQuantidade { get; set; }
        public int EmprestimoAtivoQuantidadeEmAtraso { get; set; }
        public float EmprestimoAtivoPercentualEmAtraso { get; set; }
        public int UsuariosEmprestimoParcelaEmAberto { get; set; }
        public int UsuariosEmprestimoParcelaEmAbertoEmAtraso { get; set; }
        public float UsuariosEmprestimoPercentualParcelaEmAbertoEmAtraso { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double ParcelaValorEmAberto { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double ParcelaValorEmAbertoEmAtraso { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float ParcelaPercentualValorEmAbertoEmAtraso { get; set; }
        public List<Usuario>? Usuarios { get; set; }
        public List<Emprestimo>? Emprestimos { get; set; }
        public List<Parcela>? Parcelas { get; set; }
    }
}
