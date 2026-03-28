using BlogSimples.Common.Application.Interfaces;
using BlogSimples.Postagem.Application.Interfaces;
using BlogSimples.Postagem.Infrastructure.Persistencia;
using BlogSimples.Postagem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogSimples.Postagem.Infrastructure.IoC;

public static class InjecaoDependenciaExtension
{
    public static IServiceCollection AddPostagemInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PostagemDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("Default")));

        services.AddScoped<IMigracaoDB, MigracaoDB>();

        services.AddScoped<IPostagemRepository, PostagemRepository>();

        return services;
    }
}
