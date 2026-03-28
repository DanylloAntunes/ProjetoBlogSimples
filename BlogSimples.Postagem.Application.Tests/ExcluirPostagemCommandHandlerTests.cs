using BlogSimples.Postagem.Application.Commands.ExcluirPostagem;
using BlogSimples.Postagem.Application.Interfaces;
using ErrorOr;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace BlogSimples.Postagem.Application.Tests;

public class ExcluirPostagemCommandHandlerTests
{
    private readonly IPostagemRepository _postagemRepository;
    private readonly ExcluirPostagemCommandHandler _handler;

    private static readonly string IdPostagem = "1AB";
    private static readonly Guid IdUsuarioLogado = Guid.NewGuid();

    public ExcluirPostagemCommandHandlerTests()
    {
        _postagemRepository = Substitute.For<IPostagemRepository>();
        _handler = new ExcluirPostagemCommandHandler(_postagemRepository);
    }

    private static ExcluirPostagemCommand CriarCommand(
        string id,
        Guid? idUsuarioLogado = null)
        => new(
            Id: id,
            IdUsuarioLogado: idUsuarioLogado ?? IdUsuarioLogado);

    [Fact]
    public async Task Handle_PostagemNaoEncontrada_DeveRetornarNotFound()
    {
        _postagemRepository
            .Obter(IdPostagem, Arg.Any<CancellationToken>())
            .ReturnsNull();

        var resultado = await _handler.Handle(CriarCommand(IdPostagem), CancellationToken.None);

        resultado.IsError.Should().BeTrue();
        resultado.FirstError.Type.Should().Be(ErrorType.NotFound);
        resultado.FirstError.Code.Should().Be("Postagem");
        resultado.FirstError.Description.Should().Be("Postagem não encontrada");

        await _postagemRepository
            .DidNotReceive()
            .Excluir(Arg.Any<Domain.Postagem>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_PodeExcluirRetornaErro_UsuarioNaoPodeExcluirEstaPostagem()
    {
        var postagem = Substitute.For<Domain.Postagem>();

        _postagemRepository
            .Obter(IdPostagem, Arg.Any<CancellationToken>())
            .Returns(postagem);

        var resultado = await _handler.Handle(CriarCommand(IdPostagem, Guid.NewGuid()), CancellationToken.None);

        resultado.IsError.Should().BeTrue();
        resultado.FirstError.Type.Should().Be(ErrorType.Unauthorized);
        resultado.FirstError.Code.Should().Be("Postagem");
        resultado.FirstError.Description.Should().Be("Usuário não pode excluir esta postagem");
    }

    [Fact]
    public async Task Handle_FalhaNaPersistencia_DeveRetornarFailure()
    {
        var postagem = Substitute.For<Domain.Postagem>();

        _postagemRepository
            .Obter(postagem.Id, Arg.Any<CancellationToken>())
            .Returns(postagem);

        _postagemRepository
            .Excluir(postagem, Arg.Any<CancellationToken>())
            .Returns(false);

        var resultado = await _handler.Handle(CriarCommand(postagem.Id, postagem.AutorId), CancellationToken.None);

        resultado.IsError.Should().BeTrue();
        resultado.FirstError.Type.Should().Be(ErrorType.Failure);
        resultado.FirstError.Code.Should().Be("Postagem");
        resultado.FirstError.Description.Should().Be("Erro ao excluir");
    }

    [Fact]
    public async Task Handle_FluxoValido_DeveRetornarSuccess()
    {
        var postagem = Substitute.For<Domain.Postagem>();

        _postagemRepository
            .Obter(postagem.Id, Arg.Any<CancellationToken>())
            .Returns(postagem);

        _postagemRepository
            .Excluir(postagem, Arg.Any<CancellationToken>())
            .Returns(true);

        var resultado = await _handler.Handle(CriarCommand(postagem.Id, postagem.AutorId), CancellationToken.None);

        resultado.IsError.Should().BeFalse();
        resultado.Value.Should().Be(Result.Success);
    }
}
