using BlogSimples.Common.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BlogSimples.IoC;

public class MigracaoBD
{
    public static async Task ExecuteAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var migrations = scope.ServiceProvider.GetServices<IMigracaoDB>();

        foreach (var migration in migrations)
        {
            await migration.MigracaoAsync();
        }
    }
}
