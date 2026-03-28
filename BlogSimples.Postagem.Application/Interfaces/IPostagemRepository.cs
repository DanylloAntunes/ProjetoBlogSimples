using BlogSimples.Common.Application;
using BlogSimples.Postagem.Application.Queries.ObterPostagens;

namespace BlogSimples.Postagem.Application.Interfaces;

public interface IPostagemRepository
{
    Task<string> Registrar(Domain.Postagem postagem, CancellationToken cancellationToken);
    Task<bool> Alterar(Domain.Postagem postagem, CancellationToken cancellationToken);
    Task<bool> Existe(string id, CancellationToken cancellationToken);
    Task<bool> Excluir(Domain.Postagem postagem, CancellationToken cancellationToken);
    Task<Domain.Postagem?> Obter(string id, CancellationToken cancellationToken);
    Task<PaginadoResponse<ObterPostagensResponse>> ObterPostagensPaginada(
        DateTime? ultimaDataCriacao,
        string? ultimoId, 
        int totalRegistroPagina,
        CancellationToken cancellationToken);
}
