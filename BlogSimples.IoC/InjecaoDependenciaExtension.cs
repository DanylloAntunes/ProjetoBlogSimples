using BlogSimples.Autenticacao.Application.IoC;
using BlogSimples.Autenticacao.Infrastructure.IoC;
using BlogSimples.Notificacao.Server.IoC;
using BlogSimples.Postagem.Application.IoC;
using BlogSimples.Postagem.Infrastructure.IoC;
using Microsoft.AspNetCore.Builder;
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

        services.AddNotificacao();

        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.MapSignalRNotificacao(); 

        return app;
    }
}
