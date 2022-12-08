using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FinancialSupport.Application.DTOs;

public class EmprestimoDTO
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Obrigatório informar o Id do usuário.")]
    [DisplayName("Identificação do cliente")] 
    public int? IdUsuario { get; set; }

    [Required(ErrorMessage = "Defina o valor do emprestimo.")]
    [Range(0, 9999999999999999.99)]
    [DisplayName("Valor do emprestimo")]
    [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
    public decimal Valor { get; set; }

    [Required(ErrorMessage = "Informar a data de contratação do empréstimo.")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public DateTime Data { get; set; }
    public bool Ativo { get; set; }
    public int? NumeroParcelas { get; set; }
    public List<ParcelaDTO>? Parcelas { get; set; }

    //        public Usuario Usuario { get; private set; }
}
