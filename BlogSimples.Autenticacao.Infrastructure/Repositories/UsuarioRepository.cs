using BlogSimples.Autenticacao.Application.Interfaces;
using BlogSimples.Autenticacao.Domain;
using BlogSimples.Autenticacao.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace BlogSimples.Autenticacao.Infrastructure.Repositories
{
    public class UsuarioRepository(AutenticacaoDbContext context) : IUsuarioRepository
    {
        public Task<bool> Existe(string email, CancellationToken cancellationToken)
        {
            return context.Usuarios.AnyAsync(x => x.Email == email, cancellationToken);
        }

        public Task<Usuario?> Obter(string email, CancellationToken cancellationToken)
        {
            return context.Usuarios.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }

        public async Task<string> Registrar(Usuario usuario, CancellationToken cancellationToken)
        {
            context.Usuarios.Add(usuario);

            var resultado = await context.SaveChangesAsync(cancellationToken);

            return resultado > 0 ? usuario.Id.ToString() : string.Empty;
        }
    }
}
