using BlogSimples.Autenticacao.Application.Commands.Login;
using BlogSimples.Autenticacao.Domain;
using FluentAssertions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace BlogSimples.Autenticacao.Application.Tests;

public class LoginValidadorTests
{
    private const string EmailValido = "teste@teste.com";
    private const string SenhaValida = "Senha@123";
    private readonly Usuario _usuarioFake;

    public LoginValidadorTests()
    {
        var hasher = new PasswordHasher<Usuario>();
        _usuarioFake = new Usuario { Nome = "teste", Email = EmailValido };
        _usuarioFake.DefinirSenha(hasher.HashPassword(_usuarioFake, SenhaValida));
    }

    [Fact]
    public void Validator_EmailVazio_DeveRetornarErro()
    {
        var command = new LoginCommand(Email: "", Senha: SenhaValida);
        var validator = new LoginValidador();

        ValidationResult resultado = validator.Validate(command);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.PropertyName == nameof(LoginCommand.Email));
    }

    [Fact]
    public void Validator_EmailInvalido_DeveRetornarErro()
    {
        var command = new LoginCommand(Email: "nao-e-email", Senha: SenhaValida);
        var validator = new LoginValidador();

        ValidationResult resultado = validator.Validate(command);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.PropertyName == nameof(LoginCommand.Email));
    }

    [Fact]
    public void Validator_SenhaVazia_DeveRetornarErro()
    {
        var command = new LoginCommand(Email: EmailValido, Senha: "");
        var validator = new LoginValidador();

        ValidationResult resultado = validator.Validate(command);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.PropertyName == nameof(LoginCommand.Senha));
    }

    [Theory]
    [InlineData("")]
    [InlineData("abc")]
    [InlineData("12345")]
    public void Validator_SenhaAbaixoDoMinimo_DeveRetornarErro(string senha)
    {
        var command = new LoginCommand(Email: EmailValido, Senha: senha);
        var validator = new LoginValidador();

        ValidationResult resultado = validator.Validate(command);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.PropertyName == nameof(LoginCommand.Senha));
    }

    [Fact]
    public void Validator_DadosValidos_DevePassar()
    {
        var command = new LoginCommand(Email: EmailValido, Senha: SenhaValida);
        var validator = new LoginValidador();

        ValidationResult resultado = validator.Validate(command);

        resultado.IsValid.Should().BeTrue();
        resultado.Errors.Should().BeEmpty();
    }
}
