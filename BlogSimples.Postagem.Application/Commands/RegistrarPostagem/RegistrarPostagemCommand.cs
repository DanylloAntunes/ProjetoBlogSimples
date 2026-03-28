using ErrorOr;
using MediatR;

namespace BlogSimples.Postagem.Application.Commands.RegistrarPostagem;

public record RegistrarPostagemCommand(string Titulo, string Conteudo, Guid IdUsuarioLogado) : IRequest<ErrorOr<Success>>;

