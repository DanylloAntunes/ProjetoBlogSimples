using BlogSimples.Common.Eventos;
using BlogSimples.Postagem.Application.Interfaces;
using ErrorOr;
using MediatR;

namespace BlogSimples.Postagem.Application.Commands.RegistrarPostagem;

public class RegistrarPostagemCommandHandler(IPostagemRepository postagemRepository, IMediator mediator) : IRequestHandler<RegistrarPostagemCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(RegistrarPostagemCommand request, CancellationToken cancellationToken)
    {
        var postagem = Domain.Postagem.Criar(request.Titulo, request.Conteudo, request.IdUsuarioLogado);

        if (postagem.IsError)
            return postagem.Errors;

        var identificador = await postagemRepository.Registrar(postagem.Value, cancellationToken);

        if (string.IsNullOrEmpty(identificador))
            return Error.Failure("Usuario", "Erro ao persistir");

        await EnviarNotificao(request.Titulo, request.NomeUsuario);

        return Result.Success;
    }

    private async Task EnviarNotificao(string titulo, string nomeUsuario)
    {
        await mediator.Publish(new NotificarEvent($"Uma nova postagem de {nomeUsuario}", titulo));
    }
}
