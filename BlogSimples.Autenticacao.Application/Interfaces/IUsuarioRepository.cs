using BlogSimples.Autenticacao.Domain;

namespace BlogSimples.Autenticacao.Application.Interfaces;

public interface IUsuarioRepository
{
    Task<string> Registrar(Usuario usuario, CancellationToken cancellationToken);
    Task<Usuario?> Obter(string email, CancellationToken cancellationToken);
    Task<bool> Existe(string email, CancellationToken cancellationToken);
}
