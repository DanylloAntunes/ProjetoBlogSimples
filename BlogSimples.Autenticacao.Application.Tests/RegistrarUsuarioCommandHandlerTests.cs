using BlogSimples.Autenticacao.Application.Commands.RegistrarUsuario;
using BlogSimples.Autenticacao.Application.Interfaces;
using BlogSimples.Autenticacao.Domain;
using ErrorOr;
using NSubstitute;
using FluentAssertions;

namespace BlogSimples.Autenticacao.Application.Tests;

public class RegistrarUsuarioCommandHandlerTests
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly RegistrarUsuarioCommandHandler _handler;

    private const string EmailValido = "teste@teste.com";
    private const string NomeValido = "Ana Lima";
    private const string SenhaValida = "Senha@123";
    private const string IdentificadorFake = "usr-abc-123";

    public RegistrarUsuarioCommandHandlerTests()
    {
        _usuarioRepository = Substitute.For<IUsuarioRepository>();
        _handler = new RegistrarUsuarioCommandHandler(_usuarioRepository);
    }

    [Fact]
    public async Task Handle_EmailJaEmUso_DeveRetornarConflict()
    {
        _usuarioRepository
            .Existe(EmailValido, Arg.Any<CancellationToken>())
            .Returns(true);

        var command = new RegistrarUsuarioCommand(NomeValido, EmailValido, SenhaValida);

        ErrorOr<RegistrarUsuarioResponse> resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.IsError.Should().BeTrue();
        resultado.FirstError.Type.Should().Be(ErrorType.Conflict);
        resultado.FirstError.Code.Should().Be("Usuario");
    }

    [Fact]
    public async Task Handle_EmailJaEmUso_NaoDeveRegistrar()
    {
        _usuarioRepository
            .Existe(EmailValido, Arg.Any<CancellationToken>())
            .Returns(true);

        var command = new RegistrarUsuarioCommand(NomeValido, EmailValido, SenhaValida);

        await _handler.Handle(command, CancellationToken.None);

        await _usuarioRepository
            .DidNotReceive()
            .Registrar(Arg.Any<Usuario>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_FalhaNaPersistencia_DeveRetornarFailure()
    {
        _usuarioRepository
            .Existe(EmailValido, Arg.Any<CancellationToken>())
            .Returns(false);

        _usuarioRepository
            .Registrar(Arg.Any<Usuario>(), Arg.Any<CancellationToken>())
            .Returns(string.Empty);

        var command = new RegistrarUsuarioCommand(NomeValido, EmailValido, SenhaValida);

        ErrorOr<RegistrarUsuarioResponse> resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.IsError.Should().BeTrue();
        resultado.FirstError.Type.Should().Be(ErrorType.Failure);
        resultado.FirstError.Code.Should().Be("Usuario");
    }

    [Fact]
    public async Task Handle_RegistroValido_DeveRetornarIdentificador()
    {
        _usuarioRepository
            .Existe(EmailValido, Arg.Any<CancellationToken>())
            .Returns(false);

        _usuarioRepository
            .Registrar(Arg.Any<Usuario>(), Arg.Any<CancellationToken>())
            .Returns(IdentificadorFake);

        var command = new RegistrarUsuarioCommand(NomeValido, EmailValido, SenhaValida);

        ErrorOr<RegistrarUsuarioResponse> resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.IsError.Should().BeFalse();
        IdentificadorFake.Should().Be(resultado.Value.Id);
    }

    [Fact]
    public async Task Handle_RegistroValido_DevePersistirComEmailENomeCorretos()
    {
        _usuarioRepository
            .Existe(EmailValido, Arg.Any<CancellationToken>())
            .Returns(false);

        _usuarioRepository
            .Registrar(Arg.Any<Usuario>(), Arg.Any<CancellationToken>())
            .Returns(IdentificadorFake);

        var command = new RegistrarUsuarioCommand(NomeValido, EmailValido, SenhaValida);

        await _handler.Handle(command, CancellationToken.None);

        await _usuarioRepository
            .Received(1)
            .Registrar(
                Arg.Is<Usuario>(u => u.Email == EmailValido && u.Nome == NomeValido),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_RegistroValido_DevePersistirComSenhaHasheada()
    {
        _usuarioRepository
            .Existe(EmailValido, Arg.Any<CancellationToken>())
            .Returns(false);

        _usuarioRepository
            .Registrar(Arg.Any<Usuario>(), Arg.Any<CancellationToken>())
            .Returns(IdentificadorFake);

        var command = new RegistrarUsuarioCommand(NomeValido, EmailValido, SenhaValida);

        await _handler.Handle(command, CancellationToken.None);

        await _usuarioRepository
            .Received(1)
            .Registrar(
                Arg.Is<Usuario>(u => u.Senha != SenhaValida && !string.IsNullOrEmpty(u.Senha)),
                Arg.Any<CancellationToken>());
    }
}