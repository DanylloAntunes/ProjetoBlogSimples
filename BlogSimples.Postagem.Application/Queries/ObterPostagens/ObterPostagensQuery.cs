using BlogSimples.Common.Application;
using ErrorOr;
using MediatR;

namespace BlogSimples.Postagem.Application.Queries.ObterPostagens;

public record ObterPostagensQuery(DateTime? UltimaDataCriacao, string? UltimoId, int? TotalRegistroPagina) : IRequest<ErrorOr<PaginadoResponse<ObterPostagensResponse>>>;

