using BlogSimples.Common.Application;
using BlogSimples.Postagem.Application.Interfaces;
using ErrorOr;
using MediatR;

namespace BlogSimples.Postagem.Application.Queries.ObterPostagens;

public class ObterPostagensHandler(IPostagemRepository postagemRepository) : IRequestHandler<ObterPostagensQuery, ErrorOr<PaginadoResponse<ObterPostagensResponse>>>
{
    public async Task<ErrorOr<PaginadoResponse<ObterPostagensResponse>>> Handle(ObterPostagensQuery request, CancellationToken cancellationToken)
    {
        return await postagemRepository.ObterPostagensPaginada(
            request.UltimaDataCriacao, 
            request.UltimoId,
            ObterTotalRegistroPagina(request.TotalRegistroPagina), 
            cancellationToken);
    }

    private int ObterTotalRegistroPagina(int? totalRegistro) => !totalRegistro.HasValue || totalRegistro.Value == 0 ? 10 : totalRegistro.Value;
}
