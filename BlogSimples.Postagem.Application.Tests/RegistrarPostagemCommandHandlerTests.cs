using BlogSimples.Postagem.Application.Commands.RegistrarPostagem;
using BlogSimples.Postagem.Application.Interfaces;
using ErrorOr;
using FluentAssertions;
using NSubstitute;

namespace BlogSimples.Postagem.Application.Tests;

public class RegistrarPostagemCommandHandlerTests
{
    private readonly IPostagemRepository _postagemRepository;
    private readonly RegistrarPostagemCommandHandler _handler;

    private static readonly Guid IdUsuarioLogado = Guid.NewGuid();
    private const string TituloValido = "Título da postagem";
    private const string ConteudoValido = "Conteúdo da postagem";
    private const string IdentificadorFake = "post-abc-123";

    public RegistrarPostagemCommandHandlerTests()
    {
        _postagemRepository = Substitute.For<IPostagemRepository>();
        _handler = new RegistrarPostagemCommandHandler(_postagemRepository);
    }

    private static RegistrarPostagemCommand CriarCommand(
        string? titulo = null,
        string? conteudo = null,
        Guid? idUsuarioLogado = null)
        => new(
            Titulo: titulo ?? TituloValido,
            Conteudo: conteudo ?? ConteudoValido,
            IdUsuarioLogado: idUsuarioLogado ?? IdUsuarioLogado);

    [Fact]
    public async Task Handle_RepositorioRetornaVazio_DeveRetornarFailure()
    {
        _postagemRepository
            .Registrar(Arg.Any<Domain.Postagem>(), Arg.Any<CancellationToken>())
            .Returns(string.Empty);

        var command = CriarCommand();

        var resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.IsError.Should().BeTrue();
        resultado.FirstError.Type.Should().Be(ErrorType.Failure);
        resultado.FirstError.Code.Should().Be("Usuario");
        resultado.FirstError.Description.Should().Be("Erro ao persistir");
    }

    [Fact]
    public async Task Handle_RepositorioRetornaNulo_DeveRetornarFailure()
    {
        _postagemRepository
            .Registrar(Arg.Any<Domain.Postagem>(), Arg.Any<CancellationToken>())
            .Returns((string?)null!);

        var command = CriarCommand();

        var resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.IsError.Should().BeTrue();
        resultado.FirstError.Type.Should().Be(ErrorType.Failure);
    }

    [Fact]
    public async Task Handle_FluxoValido_DeveRetornarSuccess()
    {
        _postagemRepository
            .Registrar(Arg.Any<Domain.Postagem>(), Arg.Any<CancellationToken>())
            .Returns(IdentificadorFake);

        var command = CriarCommand();

        var resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.IsError.Should().BeFalse();
        resultado.Value.Should().Be(Result.Success);

        await _postagemRepository
            .Received(1)
            .Registrar(
                Arg.Is<Domain.Postagem>(p =>
                    p.Titulo == TituloValido &&
                    p.Conteudo == ConteudoValido &&
                    p.AutorId == IdUsuarioLogado),
                Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData("Título A", "Conteúdo A")]
    [InlineData("Título B", "Conteúdo B")]
    [InlineData("Título longo com mais de vinte caracteres", "Conteúdo extenso da postagem")]
    public async Task Handle_VariasCombinacaoValidas_DeveRetornarSuccess(
        string titulo,
        string conteudo)
    {
        _postagemRepository
            .Registrar(Arg.Any<Domain.Postagem>(), Arg.Any<CancellationToken>())
            .Returns(IdentificadorFake);

        var command = CriarCommand(titulo: titulo, conteudo: conteudo);

        var resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.IsError.Should().BeFalse();
        resultado.Value.Should().Be(Result.Success);
    }
}