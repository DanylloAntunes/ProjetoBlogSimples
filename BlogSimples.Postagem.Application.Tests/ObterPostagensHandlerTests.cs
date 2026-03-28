using BlogSimples.Common.Application;
using BlogSimples.Postagem.Application.Interfaces;
using BlogSimples.Postagem.Application.Queries.ObterPostagens;
using FluentAssertions;
using NSubstitute;

namespace BlogSimples.Postagem.Application.Tests;

public class ObterPostagensHandlerTests
{
    private readonly IPostagemRepository _postagemRepository;
    private readonly ObterPostagensHandler _handler;

    private static readonly DateTime UltimaDataCriacao = DateTime.UtcNow.AddDays(-1);
    private static readonly string UltimoId = "2ABC";

    public ObterPostagensHandlerTests()
    {
        _postagemRepository = Substitute.For<IPostagemRepository>();
        _handler = new ObterPostagensHandler(_postagemRepository);
    }

    private static ObterPostagensQuery CriarQuery(
        DateTime? ultimaDataCriacao = null,
        string? ultimoId = null,
        int? totalRegistroPagina = null)
        => new(
            UltimaDataCriacao: ultimaDataCriacao,
            UltimoId: ultimoId,
            TotalRegistroPagina: totalRegistroPagina);

    private static PaginadoResponse<ObterPostagensResponse> CriarRespostaFake(int total = 2)
    {
        var itens = Enumerable
            .Range(1, total)
            .Select(i => new ObterPostagensResponse(
                Id: "{i}ABC",
                Titulo: $"Título {i}",
                Conteudo: $"Conteúdo {i}",
                DataCriacao: DateTime.UtcNow.AddDays(-i)))
            .ToList();

        return new PaginadoResponse<ObterPostagensResponse>(itens, null, string.Empty, false);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    public async Task Handle_TotalRegistro_DeveUsarPaginacaoPadraoDeDez(int? total)
    {
        var query = CriarQuery(totalRegistroPagina: total);

        _postagemRepository
            .ObterPostagensPaginada(
                Arg.Any<DateTime?>(),
                Arg.Any<string>(),
                Arg.Any<int>(),
                Arg.Any<CancellationToken>())
            .Returns(CriarRespostaFake());

        await _handler.Handle(query, CancellationToken.None);

        await _postagemRepository
            .Received(1)
            .ObterPostagensPaginada(
                Arg.Any<DateTime?>(),
                Arg.Any<string>(),
                10,
                Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(20)]
    [InlineData(50)]
    public async Task Handle_TotalRegistroValido_DeveUsarValorInformado(int totalRegistro)
    {
        var query = CriarQuery(totalRegistroPagina: totalRegistro);

        _postagemRepository
            .ObterPostagensPaginada(
                Arg.Any<DateTime?>(),
                Arg.Any<string>(),
                Arg.Any<int>(),
                Arg.Any<CancellationToken>())
            .Returns(CriarRespostaFake());

        await _handler.Handle(query, CancellationToken.None);

        await _postagemRepository
            .Received(1)
            .ObterPostagensPaginada(
                Arg.Any<DateTime?>(),
                Arg.Any<string>(),
                totalRegistro,
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ComCursor_DeveRepassarUltimaDataCriacaoEUltimoId()
    {
        var query = CriarQuery(
            ultimaDataCriacao: UltimaDataCriacao,
            ultimoId: UltimoId,
            totalRegistroPagina: 10);

        _postagemRepository
            .ObterPostagensPaginada(
                Arg.Any<DateTime?>(),
                Arg.Any<string>(),
                Arg.Any<int>(),
                Arg.Any<CancellationToken>())
            .Returns(CriarRespostaFake());

        await _handler.Handle(query, CancellationToken.None);

        await _postagemRepository
            .Received(1)
            .ObterPostagensPaginada(
                UltimaDataCriacao,
                UltimoId,
                10,
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_RepositorioRetornaPostagens_DeveRetornarMesmoResultado()
    {
        var respostaEsperada = CriarRespostaFake(total: 3);

        _postagemRepository
            .ObterPostagensPaginada(
                Arg.Any<DateTime?>(),
                Arg.Any<string>(),
                Arg.Any<int>(),
                Arg.Any<CancellationToken>())
            .Returns(respostaEsperada);

        var query = CriarQuery();
        var resultado = await _handler.Handle(query, CancellationToken.None);

        resultado.IsError.Should().BeFalse();
        resultado.Value.Should().BeEquivalentTo(respostaEsperada);
    }

    [Fact]
    public async Task Handle_RepositorioRetornaListaVazia_DeveRetornarSucesso()
    {
        var respostaVazia = new PaginadoResponse<ObterPostagensResponse>([], null, string.Empty, false);

        _postagemRepository
            .ObterPostagensPaginada(
                Arg.Any<DateTime?>(),
                Arg.Any<string>(),
                Arg.Any<int>(),
                Arg.Any<CancellationToken>())
            .Returns(respostaVazia);

        var query = CriarQuery();
        var resultado = await _handler.Handle(query, CancellationToken.None);

        resultado.IsError.Should().BeFalse();
        resultado.Value.Items.Should().BeEmpty();
    }
}
