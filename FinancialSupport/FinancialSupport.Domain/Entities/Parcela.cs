using FinancialSupport.Domain.Validation;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace FinancialSupport.Domain.Entities
{
    public class Parcela : Entity
    {
        public int? IdEmprestimo { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime DataParcela { get; set; }
        public decimal ValorParcela { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? DataPagamento { get; set; }            // ######### ATENÇÃO aqui e nas parcelas PRIVATE
        public decimal? ValorPagamento { get; set; }
        public Emprestimo? Emprestimo { get; set; }
        public Parcela() { }
        public Parcela(int id)
        {
            DomainExceptionValidation.When(id < 0, "Id do parcela inválido");
            Id = id;
        }
        public Parcela(int id, int idEmprestimo)
        {
            DomainExceptionValidation.When(id < 0, "Id do parcela inválido");
            DomainExceptionValidation.When(idEmprestimo < 0, "Id do parcela do emprestimo inválido");
            Id = id;
            IdEmprestimo = idEmprestimo;
        }
        public Parcela(int id, int idEmprestimo, DateTime dataParcela, decimal valorParcela, DateTime dataPagamento, decimal valorPagamento)
        {
            DomainExceptionValidation.When(id < 0, "Id da parcela inválido");
            DomainExceptionValidation.When(idEmprestimo < 0, "Id do emprestimo inválido");
            DomainExceptionValidation.When(dataParcela.ToString().IsNullOrEmpty(), "dataParcela inválida");
            
            Id = id;
            IdEmprestimo = idEmprestimo;
            DataParcela = dataParcela;
            ValorParcela = valorParcela;
            DataPagamento = dataPagamento;
            ValorPagamento = valorPagamento;
        }
        public void Update(int id)
        {
            DomainExceptionValidation.When(id < 0, "Id do parcela inválido");
            Id = id;
        }
    }
}
