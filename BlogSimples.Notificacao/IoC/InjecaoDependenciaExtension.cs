using BlogSimples.Notificacao.Server.Hub;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace BlogSimples.Notificacao.Server.IoC;

public static class InjecaoDependenciaExtension
{
    public static IServiceCollection AddNotificacao(this IServiceCollection services)
    {
        services.AddMediatR(mediat =>
        {
            mediat.RegisterServicesFromAssemblies(typeof(InjecaoDependenciaExtension).Assembly);
        });

        services.AddSignalR()
            .AddHubOptions<NotificacaoHub>(options =>
            {
                options.KeepAliveInterval = TimeSpan.FromSeconds(15);
                options.HandshakeTimeout = TimeSpan.FromSeconds(15);
            })
            .AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
            });

        return services;
    }

    public static WebApplication MapSignalRNotificacao(this WebApplication app)
    {
        app.MapHub<NotificacaoHub>("/hubs/notificacao");
        return app;
    }
}
