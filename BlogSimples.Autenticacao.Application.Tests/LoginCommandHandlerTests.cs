using BlogSimples.Autenticacao.Application.Commands.Login;
using BlogSimples.Autenticacao.Application.Interfaces;
using BlogSimples.Autenticacao.Domain;
using ErrorOr;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace BlogSimples.Autenticacao.Application.Tests;

public class LoginCommandHandlerTests
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITokenGeneratorServices _tokenGenerator;
    private readonly LoginCommandHandler _handler;

    private const string EmailValido = "teste@teste.com";
    private const string SenhaValida = "Senha@123";
    private const string TokenFake = "eyJhbGciOiJIUzI1NiJ9.fake.token";

    private readonly Usuario _usuarioFake;

    public LoginCommandHandlerTests()
    {
        _usuarioRepository = Substitute.For<IUsuarioRepository>();
        _tokenGenerator = Substitute.For<ITokenGeneratorServices>();
        _handler = new LoginCommandHandler(_usuarioRepository, _tokenGenerator);

        var hasher = new PasswordHasher<Usuario>();
        _usuarioFake = new Usuario { Nome = "teste", Email = EmailValido };
        _usuarioFake.DefinirSenha(hasher.HashPassword(_usuarioFake, SenhaValida));
    }

    [Fact]
    public async Task Handle_UsuarioNaoEncontrado_DeveRetornar401()
    {
        _usuarioRepository
            .Obter(EmailValido, Arg.Any<CancellationToken>())
            .Returns((Usuario?)null);

        var command = new LoginCommand(EmailValido, SenhaValida);

        ErrorOr<LoginResponse> resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.IsError.Should().BeTrue();
        resultado.FirstError.Type.Should().Be(ErrorType.Unauthorized);
        resultado.FirstError.Code.Should().Be("Login");
    }

    [Fact]
    public async Task Handle_SenhaIncorreta_DeveRetornar401()
    {
        _usuarioRepository
            .Obter(EmailValido, Arg.Any<CancellationToken>())
            .Returns(_usuarioFake);

        var command = new LoginCommand(EmailValido, "SenhaErrada!");

        ErrorOr<LoginResponse> resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.IsError.Should().BeTrue();
        Assert.True(resultado.IsError);
        resultado.FirstError.Type.Should().Be(ErrorType.Unauthorized);
        _tokenGenerator
            .DidNotReceive()
            .ObterToken(Arg.Any<Usuario>());
    }

    [Fact]
    public async Task Handle_LoginValido_DeveRetornarToken()
    {
        _usuarioRepository
            .Obter(EmailValido, Arg.Any<CancellationToken>())
            .Returns(_usuarioFake);

        _tokenGenerator
            .ObterToken(_usuarioFake)
            .Returns(TokenFake);

        var command = new LoginCommand(EmailValido, SenhaValida);

        ErrorOr<LoginResponse> resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.IsError.Should().BeFalse();
        resultado.Value.Token.Should().Be(TokenFake);
    }

    [Fact]
    public async Task Handle_LoginValido_DeveGerarTokenParaOUsuarioCorreto()
    {
        _usuarioRepository
            .Obter(EmailValido, Arg.Any<CancellationToken>())
            .Returns(_usuarioFake);

        _tokenGenerator
            .ObterToken(_usuarioFake)
            .Returns(TokenFake);

        var command = new LoginCommand(EmailValido, SenhaValida);

        await _handler.Handle(command, CancellationToken.None);

        _tokenGenerator
            .Received(1)
            .ObterToken(_usuarioFake);
    }

    [Fact]
    public async Task Handle_SenhaSemRehash_NaoDeveRegistrar()
    {
        _usuarioRepository
            .Obter(EmailValido, Arg.Any<CancellationToken>())
            .Returns(_usuarioFake);

        _tokenGenerator
            .ObterToken(_usuarioFake)
            .Returns(TokenFake);

        var command = new LoginCommand(EmailValido, SenhaValida);

        await _handler.Handle(command, CancellationToken.None);

        await _usuarioRepository
            .DidNotReceive()
            .Registrar(Arg.Any<Usuario>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_UsuarioNaoEncontrado_NaoDeveConsultarToken()
    {
        _usuarioRepository
            .Obter(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((Usuario?)null);

        var command = new LoginCommand(EmailValido, SenhaValida);

        await _handler.Handle(command, CancellationToken.None);

        _tokenGenerator
            .DidNotReceive()
            .ObterToken(Arg.Any<Usuario>());
    }

    [Theory]
    [InlineData("outro@email.com")]
    [InlineData("OUTRO@EMAIL.COM")]
    public async Task Handle_EmailDiferente_DeveRetornar401(string email)
    {
        _usuarioRepository
            .Obter(email, Arg.Any<CancellationToken>())
            .Returns((Usuario?)null);

        var command = new LoginCommand(email, SenhaValida);

        ErrorOr<LoginResponse> resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.IsError.Should().BeTrue();
        resultado.FirstError.Type.Should().Be(ErrorType.Unauthorized);
    }
}
