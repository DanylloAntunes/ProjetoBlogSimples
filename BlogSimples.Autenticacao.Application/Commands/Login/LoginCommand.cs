using ErrorOr;
using MediatR;

namespace BlogSimples.Autenticacao.Application.Commands.Login;

public record LoginCommand(string Email, string Senha) : IRequest<ErrorOr<LoginResponse>>;
