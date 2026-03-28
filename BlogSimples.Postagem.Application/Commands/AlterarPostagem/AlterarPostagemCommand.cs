using ErrorOr;
using MediatR;

namespace BlogSimples.Postagem.Application.Commands.AlterarPostagem;

public record AlterarPostagemCommand(string Id, string Titulo, string Conteudo, Guid IdUsuarioLogado) : IRequest<ErrorOr<Success>>;

