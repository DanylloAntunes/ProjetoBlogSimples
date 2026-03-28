using BlogSimples.Api.Configuracao;
using BlogSimples.Autenticacao.Application.Commands.RegistrarUsuario;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogSimples.Api.Controllers;

public static class UsuarioEndpoint 
{
    public static void MapEndpointsUsuario(this IEndpointRouteBuilder app)
    {
        var v1 = app.MapGroup("api/v1/usuario")
                    .WithTags("Usuário");

        v1.MapPost("/", Registrar)
             .WithName("Registrar")
             .Produces<RegistrarUsuarioResponse>(StatusCodes.Status200OK)
             .ProducesValidationProblem(); 
    }

    private static async Task<IResult> Registrar(IMediator mediator, RegistrarUsuarioCommand registrarUsuarioCommand)
    {
        var resultado = await mediator.Send(registrarUsuarioCommand);

        return resultado.ToResult(); 
    }
}
