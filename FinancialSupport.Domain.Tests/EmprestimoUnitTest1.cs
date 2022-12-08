using FinancialSupport.Domain.Entities;
using FluentAssertions;


namespace FinancialSupport.Domain.Tests
{
    public class EmprestimoUnitTest1
    {
        [Fact(DisplayName = "Cria Empr�stimo v�lido, sem Id")]
        public void CreateEmprestimo_ComParametrosValidos_RetornaValidState()
        {
            Action action = () => new Emprestimo(01, 01, 10, DateTime.Parse("04/11/2022"), true);
            action.Should().NotThrow<FinancialSupport.Domain.Validation.DomainExceptionValidation>();
        }

        [Fact(DisplayName = "Cria Empr�stimo v�lido, com Id")]
        public void CreateEmprestimo_ComParametrosValidosMaisId_RetornaValidState()
        {
            Action action = () => new Emprestimo(1, 1, 10, DateTime.Parse("04/11/2022"), true);
            action.Should().NotThrow<FinancialSupport.Domain.Validation.DomainExceptionValidation>();
        }

        [Fact(DisplayName = "Cria Empr�stimo inv�lido, com Id negativo")]
        public void CreateEmprestimo_ComParametrosIdnegativo_RetornaMsgErro()
        {
            Action action = () => new Emprestimo(-1, 1, 10, DateTime.Parse("04/11/2022"), true);
            action.Should().Throw<FinancialSupport.Domain.Validation.DomainExceptionValidation>();
        }

        [Fact(DisplayName = "Cria Empr�stimo inv�lido, Id usu�rio inv�lido")]
        public void CreateEmprestimo_ComParametrosInvalidoComUsuarioNegativo_RetornaMsgErro()
        {
            Action action = () => new Emprestimo(1, -1, 10, DateTime.Parse("04/11/2022"), true);
            action.Should().Throw<FinancialSupport.Domain.Validation.DomainExceptionValidation>();
        }
    }
}