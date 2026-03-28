using BlogSimples.Autenticacao.Application.Commands.RegistrarUsuario;
using FluentAssertions;
using FluentValidation.Results;

namespace BlogSimples.Autenticacao.Application.Tests;

public class RegistrarUsuarioValidadorTests
{
    private const string EmailValido = "teste@teste.com";
    private const string NomeValido = "Ana Lima";
    private const string SenhaValida = "Senha@123";

    [Fact]
    public void Validator_EmailVazio_DeveRetornarErro()
    {
        var command = new RegistrarUsuarioCommand(Email: "", Nome: NomeValido, Senha: SenhaValida);
        var validator = new RegistrarUsuarioValidador();

        ValidationResult resultado = validator.Validate(command);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.PropertyName == nameof(RegistrarUsuarioCommand.Email));
    }

    [Fact]
    public void Validator_EmailInvalido_DeveRetornarErro()
    {
        var command = new RegistrarUsuarioCommand(Email: "nao-e-email", Nome: NomeValido, Senha: SenhaValida);
        var validator = new RegistrarUsuarioValidador();

        ValidationResult resultado = validator.Validate(command);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.PropertyName == nameof(RegistrarUsuarioCommand.Email));
    }

    [Fact]
    public void Validator_NomeVazio_DeveRetornarErro()
    {
        var command = new RegistrarUsuarioCommand(Email: EmailValido, Nome: "", Senha: SenhaValida);
        var validator = new RegistrarUsuarioValidador();

        ValidationResult resultado = validator.Validate(command);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.PropertyName == nameof(RegistrarUsuarioCommand.Nome));
    }

    [Theory]
    [InlineData("")]
    [InlineData("12345")]
    [InlineData("abc")]
    public void Validator_SenhaAbaixoDoMinimo_DeveRetornarErro(string senha)
    {
        var command = new RegistrarUsuarioCommand(Email: EmailValido, Nome: NomeValido, Senha: senha);
        var validator = new RegistrarUsuarioValidador();

        ValidationResult resultado = validator.Validate(command);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.PropertyName == nameof(RegistrarUsuarioCommand.Senha));
    }

    [Fact]
    public void Validator_DadosValidos_DevePassar()
    {
        var command = new RegistrarUsuarioCommand(Email: EmailValido, Nome: NomeValido, Senha: SenhaValida);
        var validator = new RegistrarUsuarioValidador();

        ValidationResult resultado = validator.Validate(command);

        resultado.IsValid.Should().BeTrue();
        resultado.Errors.Should().BeEmpty();
    }
}
