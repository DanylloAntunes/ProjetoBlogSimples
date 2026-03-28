using BlogSimples.Common.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogSimples.Postagem.Infrastructure.Persistencia;

public class MigracaoDB(PostagemDbContext context) : IMigracaoDB
{
    private readonly PostagemDbContext _context = context;

    public async Task MigracaoAsync()
    {
        var schema = _context.Model.GetDefaultSchema();

        if (!string.IsNullOrEmpty(schema))
        {
            await _context.Database.ExecuteSqlRawAsync($"CREATE SCHEMA IF NOT EXISTS \"{schema}\"");
        }

        await _context.Database.MigrateAsync();
    }
}
