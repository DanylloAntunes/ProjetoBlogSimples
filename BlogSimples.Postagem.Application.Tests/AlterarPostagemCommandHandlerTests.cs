using BlogSimples.Postagem.Application.Commands.AlterarPostagem;
using BlogSimples.Postagem.Application.Interfaces;
using ErrorOr;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace BlogSimples.Postagem.Application.Tests;


public class AlterarPostagemCommandHandlerTests
{
    private readonly IPostagemRepository _postagemRepository;
    private readonly AlterarPostagemCommandHandler _handler;

    private const string IdPostagem = "1AB";
    private static readonly Guid IdUsuarioLogado = Guid.NewGuid();
    private const string TituloValido = "Novo título";
    private const string ConteudoValido = "Novo conteúdo da postagem";

    public AlterarPostagemCommandHandlerTests()
    {
        _postagemRepository = Substitute.For<IPostagemRepository>();
        _handler = new AlterarPostagemCommandHandler(_postagemRepository);
    }

    private static AlterarPostagemCommand CriarCommand(
        string id,
        string? titulo = null,
        string? conteudo = null,
        Guid? idUsuarioLogado = null)
        => new(
            Id: id,
            Titulo: titulo ?? TituloValido,
            Conteudo: conteudo ?? ConteudoValido,
            IdUsuarioLogado: idUsuarioLogado ?? IdUsuarioLogado);

    private static Domain.Postagem CriarPostagemFake()
    {
        var postagem = Domain.Postagem.Criar("Título original", "Conteúdo original", IdUsuarioLogado);

        return postagem.Value;
    }

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

        await _postagemRepository
             .DidNotReceive()
             .Alterar(Arg.Any<Domain.Postagem>(), Arg.Any<CancellationToken>());
    }


    [Fact]
    public async Task Handle_AtualizarRetornaErro_UsuarioNaoPodeAlterar()
    {
        var postagem = CriarPostagemFake();

        _postagemRepository
            .Obter(postagem.Id, Arg.Any<CancellationToken>())
            .Returns(postagem);

        var resultado = await _handler.Handle(CriarCommand(postagem.Id, idUsuarioLogado: Guid.NewGuid()), CancellationToken.None);

        resultado.IsError.Should().BeTrue();
        resultado.FirstError.Type.Should().Be(ErrorType.Unauthorized);
        resultado.FirstError.Description.Should().Be("Usuário não pode alterar esta postagem");

        await _postagemRepository
            .DidNotReceive()
            .Alterar(Arg.Any<Domain.Postagem>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_FalhaNaPersistencia_DeveRetornarFailure()
    {
        var postagem = CriarPostagemFake();

        _postagemRepository
            .Obter(IdPostagem, Arg.Any<CancellationToken>())
            .Returns(postagem);

        _postagemRepository
            .Alterar(postagem, Arg.Any<CancellationToken>())
            .Returns(false);

        var resultado = await _handler.Handle(CriarCommand(IdPostagem), CancellationToken.None);

        resultado.IsError.Should().BeTrue();
        resultado.FirstError.Type.Should().Be(ErrorType.Failure);
        resultado.FirstError.Code.Should().Be("Postagem");
    }

    // ── Fluxo completo com sucesso ────────────────────────────────────────────

    [Fact]
    public async Task Handle_FluxoValido_DeveRetornarSuccess()
    {
        var postagem = CriarPostagemFake();

        _postagemRepository
            .Obter(postagem.Id, Arg.Any<CancellationToken>())
            .Returns(postagem);

        _postagemRepository
            .Alterar(postagem, Arg.Any<CancellationToken>())
            .Returns(true);

        var resultado = await _handler.Handle(CriarCommand(postagem.Id, idUsuarioLogado: postagem.AutorId), CancellationToken.None);

        resultado.IsError.Should().BeFalse();
        resultado.Value.Should().Be(Result.Success);
    }

    [Fact]
    public async Task Handle_FluxoValido_DeveBuscarPostagemPeloIdCorreto()
    {
        var postagem = CriarPostagemFake();

        _postagemRepository
            .Obter(postagem.Id, Arg.Any<CancellationToken>())
            .Returns(postagem);

        _postagemRepository
            .Alterar(postagem, Arg.Any<CancellationToken>())
            .Returns(true);

        await _handler.Handle(CriarCommand(postagem.Id, idUsuarioLogado: postagem.AutorId), CancellationToken.None);

        await _postagemRepository
            .Received(1)
            .Obter(postagem.Id, Arg.Any<CancellationToken>());
    }
}
