using BlogSimples.Autenticacao.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogSimples.Autenticacao.Infrastructure.Persistencia;

public class AutenticacaoDbContext(DbContextOptions<AutenticacaoDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("autenticacao");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AutenticacaoDbContext).Assembly);
    }
}
