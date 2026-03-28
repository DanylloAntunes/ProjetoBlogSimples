using ErrorOr;
using MediatR;

namespace BlogSimples.Postagem.Application.Commands.ExcluirPostagem;

public record ExcluirPostagemCommand(string Id, Guid IdUsuarioLogado) : IRequest<ErrorOr<Success>>;


