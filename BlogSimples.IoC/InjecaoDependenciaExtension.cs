using BlogSimples.Autenticacao.Application.IoC;
using BlogSimples.Autenticacao.Infrastructure.IoC;
using BlogSimples.Postagem.Application.IoC;
using BlogSimples.Postagem.Infrastructure.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogSimples.IoC;

public static class InjecaoDependenciaExtension
{
    public static IServiceCollection ConfigureDependencias(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddAutenticacaoApplication();
        services.AddAutenticacaoInfrastructure(configuration);

        services.AddPostagemApplication();
        services.AddPostagemInfrastructure(configuration);

        return services;
    }
}
