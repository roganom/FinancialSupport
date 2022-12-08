using FinancialSupport.Domain.Entities;
using FluentAssertions;

namespace FinancialSupport.Domain.Tests
{
    public class UsuarioUnitTest1
    {
        [Fact(DisplayName = "Cria Usuário válido, sem Id")]
        public void CreateUsuario_ComParametroValido_RetornaValidState()
        {
            Action action = () => new Usuario("Zenóbio");
            action.Should().NotThrow<FinancialSupport.Domain.Validation.DomainExceptionValidation>();
        }

        [Fact(DisplayName = "Cria Usuário válido, com Id")]
        public void reateUsuario_ComParametroValidoMaisId_RetornaValidState()
        {
            Action action = () => new Usuario(1, "Zenóbio");
            action.Should().NotThrow<FinancialSupport.Domain.Validation.DomainExceptionValidation>();
        }


        [Fact(DisplayName = "Cria Usuário inválido, com Id, mas nome pequeno")]
        public void CreateEmprestimoUsuario_ComIdMasNomePequeno_RetornaMsgErro()
        {
            Action action = () => new Usuario(1, "Ze");
            action.Should().Throw<FinancialSupport.Domain.Validation.DomainExceptionValidation>();
        }
    }
}
