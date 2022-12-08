using FinancialSupport.Domain.Validation;
using System.ComponentModel.DataAnnotations;

namespace FinancialSupport.Domain.Entities
{
    public sealed class Emprestimo : Entity
    {
        public int? IdUsuario { get;  set; }
        public decimal Valor { get;  set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? Data { get; set; }
        public bool Ativo { get;  set; }
        public int NumeroParcelas { get;  set; }

        public List<Parcela>? Parcelas { get; set; }
        public Usuario? Usuario { get;  set; }
        public Emprestimo() { }
        public Emprestimo(int id, int usuario, decimal valor, DateTime dataEmprestimo, bool ativo)
        {
            DomainExceptionValidation.When(id < 0, "Id empréstimo inválido");
            Id = id;
            ValidateDomain(id, usuario, valor, dataEmprestimo);
        }
        public Emprestimo(int usuario, decimal valor, DateTime dataEmprestimo)
        {
            ValidateDomain(1, usuario, valor, dataEmprestimo);
        }
        public void Update(int id, int usuario, decimal valor, DateTime dataEmprestimo)
        {
            DomainExceptionValidation.When(id > 0, "Id empréstimo inválido");
            Id = id;
            ValidateDomain(id, usuario, valor, dataEmprestimo);
        }
        private void ValidateDomain(int? id, int usuario, decimal valor, DateTime dtEmprestimo)
        {
            DomainExceptionValidation.When(Id < 0, "Id usuário (empréstimo) inválido");
            DomainExceptionValidation.When(usuario < 0, "Id usuário empréstimo inválido");
            DomainExceptionValidation.When(valor < 0, "Valor do empréstimo inválido");
            IdUsuario = usuario;
            Valor = valor;
            Data = dtEmprestimo;
        }
    }    
}

