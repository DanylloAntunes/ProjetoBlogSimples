using BlogSimples.Api.Configuracao;
using BlogSimples.Autenticacao.Application.Commands.Login;
using BlogSimples.Autenticacao.Application.Commands.RegistrarUsuario;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogSimples.Api.Controllers;

public static class AutenticarEndpoint
{
    public static void MapEndpointsAutenticar(this IEndpointRouteBuilder app)
    {
        var v1 = app.MapGroup("api/v1/autenticar")
                    .WithTags("Autenticação"); 

        v1.MapPost("/", Logar)
             .WithName("Logar")
             .Produces<RegistrarUsuarioResponse>(StatusCodes.Status200OK)
             .ProducesValidationProblem();
    }

    private static async Task<IResult> Logar(IMediator mediator, LoginCommand loginCommand)
    {
        var resultado = await mediator.Send(loginCommand);

        return resultado.ToResult();
    }
}
