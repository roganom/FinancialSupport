using FinancialSupport.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialSupport.Domain.Entities
{
    public sealed class Usuario : Entity
    {
        public string? Nome { get; set; }
        public string? Foto { get; set; }
        public decimal? Limite { get; set; }
        public decimal? LimiteDisponivel { get; set; }
        public List<Emprestimo>? Emprestimos { get; set; }
        public Usuario() { }
        public Usuario(string name)
        {
            ValidateDomain(name);
        }
        public Usuario(int id, string name)
        {
            DomainExceptionValidation.When(id < 0, "Id do usuário inválido");
            Id = id;
            ValidateDomain(name);
        }
        public Usuario(int id, string name, string foto, decimal limite)
        {
            DomainExceptionValidation.When(id < 0, "Id do usuário inválido");
            Id = id;
            ValidateDomain(name);
            Foto = foto;
            Limite = limite;
        }
        public Usuario(int id, string name, string foto, decimal limite, decimal limiteDisponivel)
        {
            DomainExceptionValidation.When(id < 0, "Id do usuário inválido");
            Id = id;
            ValidateDomain(name);
            Foto = foto;
            Limite = limite;
            DomainExceptionValidation.When(limiteDisponivel > limite, "Ajuste os limites");
            LimiteDisponivel = limiteDisponivel;
        }
        public void Update(string name)
        {
            ValidateDomain(name);
            Nome = name;
        }
        private void ValidateDomain (string name)
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(name), "Nome Inválido - nulo");

            DomainExceptionValidation.When(name?.Length < 3, "Nome Inválido - menos que 3 letras");

            Nome = name;
        }
    }
}
