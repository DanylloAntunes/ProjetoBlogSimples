using BlogSimples.Common.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogSimples.Autenticacao.Infrastructure.Persistencia;

public class MigracaoDB(AutenticacaoDbContext context) : IMigracaoDB
{
    private readonly AutenticacaoDbContext _context = context;

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
