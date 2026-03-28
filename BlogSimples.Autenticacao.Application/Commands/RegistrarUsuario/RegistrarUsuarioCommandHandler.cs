using BlogSimples.Autenticacao.Application.Interfaces;
using BlogSimples.Autenticacao.Domain;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BlogSimples.Autenticacao.Application.Commands.RegistrarUsuario;

public class RegistrarUsuarioCommandHandler(IUsuarioRepository usuarioRepository) : IRequestHandler<RegistrarUsuarioCommand, ErrorOr<RegistrarUsuarioResponse>>
{
    public async Task<ErrorOr<RegistrarUsuarioResponse>> Handle(RegistrarUsuarioCommand request, CancellationToken cancellationToken)
    {
        if (await usuarioRepository.Existe(request.Email, cancellationToken))
            return Error.Conflict("Usuario", "Email já está em uso");
        
        var usuario = new Usuario()
        {
            Email = request.Email,
            Nome = request.Nome
        };

        DefinirSenha(usuario, request.Senha);

        var identificador = await usuarioRepository.Registrar(usuario, cancellationToken);

        if (string.IsNullOrEmpty(identificador))
            return Error.Failure("Usuario", "Erro ao persistir");

        return new RegistrarUsuarioResponse(identificador);
    }

    private void DefinirSenha(Usuario usuario, string senha)
    {
        var hasher = new PasswordHasher<Usuario>();

        var hash = hasher.HashPassword(usuario, senha);

        usuario.DefinirSenha(hash);
    }
}
