using MediatR;
using ErrorOr;

namespace BlogSimples.Autenticacao.Application.Commands.RegistrarUsuario;

public record RegistrarUsuarioCommand(string Nome, string Email, string Senha) : IRequest<ErrorOr<RegistrarUsuarioResponse>>;