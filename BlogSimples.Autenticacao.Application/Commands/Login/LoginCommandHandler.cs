using BlogSimples.Autenticacao.Application.Interfaces;
using BlogSimples.Autenticacao.Domain;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BlogSimples.Autenticacao.Application.Commands.Login;

public class LoginCommandHandler(
                IUsuarioRepository usuarioRepository,
                ITokenGeneratorServices jwtTokenGeneratorServices) : IRequestHandler<LoginCommand, ErrorOr<LoginResponse>>
{
    public async Task<ErrorOr<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var usuario = await usuarioRepository.Obter(request.Email, cancellationToken);

        if (usuario is null)
            return Error.Unauthorized("Login", "Email ou senha inválidos");
        
        var hasher = new PasswordHasher<Usuario>();

        var resultadoSenha = hasher.VerifyHashedPassword(
            usuario,
            usuario.Senha,
            request.Senha);

        if (resultadoSenha.Equals(PasswordVerificationResult.Failed))
            return Error.Unauthorized("Login", "Email ou senha inválidos");
        
        await VerificaNecessidadeAtualizacaoSenhaHash(hasher, resultadoSenha, usuario, request.Senha, cancellationToken);

        var token = jwtTokenGeneratorServices.ObterToken(usuario);

        return new LoginResponse(token);
    }

    private async Task VerificaNecessidadeAtualizacaoSenhaHash(
            PasswordHasher<Usuario> hasher,
            PasswordVerificationResult resultadoSenha, 
            Usuario usuario,
            string senha,
            CancellationToken cancellationToken)
    {
        if (resultadoSenha == PasswordVerificationResult.SuccessRehashNeeded)
        {
            var novaSenhaHash = hasher.HashPassword(usuario, senha);

            usuario.DefinirSenha(novaSenhaHash);

            await usuarioRepository.Registrar(usuario, cancellationToken);
        }
    }
}