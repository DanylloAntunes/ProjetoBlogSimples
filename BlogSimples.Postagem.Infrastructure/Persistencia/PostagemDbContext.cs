using Microsoft.EntityFrameworkCore;

namespace BlogSimples.Postagem.Infrastructure.Persistencia;

public class PostagemDbContext(DbContextOptions<PostagemDbContext> options) : DbContext(options)
{
    public DbSet<Domain.Postagem> Postagem => Set<Domain.Postagem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("postagem");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostagemDbContext).Assembly);
    }
}
