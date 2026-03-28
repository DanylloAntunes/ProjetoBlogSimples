using BlogSimples.Autenticacao.Domain;

namespace BlogSimples.Autenticacao.Application.Interfaces;

public interface ITokenGeneratorServices
{
    string ObterToken(Usuario usuario);
}
