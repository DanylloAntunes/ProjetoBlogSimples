using BlogSimples.Postagem.Application.Interfaces;
using ErrorOr;
using MediatR;

namespace BlogSimples.Postagem.Application.Commands.RegistrarPostagem;

public class RegistrarPostagemCommandHandler(IPostagemRepository postagemRepository) : IRequestHandler<RegistrarPostagemCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(RegistrarPostagemCommand request, CancellationToken cancellationToken)
    {
        var postagem = Domain.Postagem.Criar(request.Titulo, request.Conteudo, request.IdUsuarioLogado);

        if (postagem.IsError)
            return postagem.Errors;

        var identificador = await postagemRepository.Registrar(postagem.Value, cancellationToken);

        if (string.IsNullOrEmpty(identificador))
            return Error.Failure("Usuario", "Erro ao persistir");

        return Result.Success;
    }
}
