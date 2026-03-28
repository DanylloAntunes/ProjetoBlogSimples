using BlogSimples.Common.Application;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BlogSimples.Postagem.Application.IoC;

public static class InjecaoDependenciaExtension
{
    public static IServiceCollection AddPostagemApplication(this IServiceCollection services)
    {
        services.AddMediatR(mediat =>
        {
            mediat.RegisterServicesFromAssemblies(typeof(InjecaoDependenciaExtension).Assembly);
            mediat.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(InjecaoDependenciaExtension).Assembly);

        return services;
    }
}
