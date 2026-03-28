using BlogSimples.Autenticacao.Application.Interfaces;
using BlogSimples.Autenticacao.Infrastructure.Persistencia;
using BlogSimples.Autenticacao.Infrastructure.Repositories;
using BlogSimples.Autenticacao.Infrastructure.TokenService;
using BlogSimples.Common.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogSimples.Autenticacao.Infrastructure.IoC;

public static class InjecaoDependenciaExtension
{
    public static IServiceCollection AddAutenticacaoInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AutenticacaoDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("Default")));

        services.AddScoped<IMigracaoDB, MigracaoDB>();

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        services.AddScoped<ITokenGeneratorServices, JwtTokenGeneratorServices>();

        return services;
    }
}
