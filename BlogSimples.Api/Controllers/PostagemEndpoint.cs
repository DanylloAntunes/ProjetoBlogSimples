using BlogSimples.Api.Configuracao;
using BlogSimples.Autenticacao.Application.Commands.RegistrarUsuario;
using BlogSimples.Postagem.Application.Commands.AlterarPostagem;
using BlogSimples.Postagem.Application.Commands.ExcluirPostagem;
using BlogSimples.Postagem.Application.Commands.RegistrarPostagem;
using BlogSimples.Postagem.Application.Queries.ObterPostagens;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogSimples.Api.Controllers;

public static class PostagemEndpoint
{
    public static void MapEndpointsPostagem(this IEndpointRouteBuilder app)
    {
        var v1 = app.MapGroup("api/v1/postagem")
                    .WithTags("Postagem");

        v1.MapPost("/", CriarPostagem)
             .WithName("Criar postagem")
             .RequireAuthorization()
             .Produces<RegistrarUsuarioResponse>(StatusCodes.Status200OK)
             .ProducesValidationProblem()
             .Produces(StatusCodes.Status401Unauthorized);

        v1.MapPut("/", AlterarPostagem)
             .WithName("Alterar postagem")
             .RequireAuthorization()
             .Produces<Success>(StatusCodes.Status200OK)
             .ProducesValidationProblem()
             .Produces(StatusCodes.Status401Unauthorized);


        v1.MapDelete("/", ExcluirPostagem)
             .WithName("Excluir postagem")
             .RequireAuthorization()
             .Produces<Success>(StatusCodes.Status200OK)
             .ProducesValidationProblem()
             .Produces(StatusCodes.Status401Unauthorized);

        v1.MapGet("/postagens", ObterPostagens)
             .WithName("Postagens paginada")
             .Produces<Success>(StatusCodes.Status200OK)
             .ProducesValidationProblem()
             .Produces(StatusCodes.Status401Unauthorized);
    }

    private static async Task<IResult> CriarPostagem(
            IMediator mediator,
            [FromBody] RegistrarPostagemRequest registrarPostagemRequest,
            ClaimsPrincipal claimsPrincipal)
    {
        var usuarioId = claimsPrincipal.ObterIdUsuario();

        if (usuarioId is null)
            return Results.Unauthorized();

        var command = new RegistrarPostagemCommand(
            registrarPostagemRequest.Titulo,
            registrarPostagemRequest.Conteudo,
            usuarioId.Value,
            claimsPrincipal.ObterNomeUsuario()
        );

        var result = await mediator.Send(command);

        return result.ToResult();
    }

    private static async Task<IResult> AlterarPostagem(
        IMediator mediator,
        [FromBody] AlterarPostagemRequest postagemRequest,
        ClaimsPrincipal claimsPrincipal)
    {
        var usuarioId = claimsPrincipal.ObterIdUsuario();

        if (usuarioId is null)
            return Results.Unauthorized();

        var command = new AlterarPostagemCommand(
            postagemRequest.Id,
            postagemRequest.Titulo,
            postagemRequest.Conteudo,
            usuarioId.Value
        );

        var result = await mediator.Send(command);

        return result.ToResult();
    }

    private static async Task<IResult> ExcluirPostagem(
        IMediator mediator,
        [FromBody] ExcluirPostagemRequest postagemRequest,
        ClaimsPrincipal claimsPrincipal)
    {
        var usuarioId = claimsPrincipal.ObterIdUsuario();

        if (usuarioId is null)
            return Results.Unauthorized();

        var command = new ExcluirPostagemCommand(
            postagemRequest.Id,
            usuarioId.Value
        );

        var result = await mediator.Send(command);

        return result.ToResult();
    }

    private static async Task<IResult> ObterPostagens(
        IMediator mediator,
        [AsParameters] ObterPostagensQuery postagemRequest)
    {
        var result = await mediator.Send(postagemRequest);

        return result.ToResult();
    }
}
