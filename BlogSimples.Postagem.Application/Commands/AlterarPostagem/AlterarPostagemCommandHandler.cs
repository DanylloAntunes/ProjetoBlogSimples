using BlogSimples.Postagem.Application.Interfaces;
using ErrorOr;
using MediatR;

namespace BlogSimples.Postagem.Application.Commands.AlterarPostagem;

public class AlterarPostagemCommandHandler(IPostagemRepository postagemRepository) : IRequestHandler<AlterarPostagemCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(AlterarPostagemCommand request, CancellationToken cancellationToken)
    {
        var postagem = await postagemRepository.Obter(request.Id, cancellationToken);

        if (postagem is null)
            return Error.NotFound("Postagem", "Postagem não encontrada");

        var atualizarPostagem = postagem.Atualizar(request.Titulo, request.Conteudo, request.IdUsuarioLogado);

        if (atualizarPostagem.IsError)
            return atualizarPostagem.Errors;

        var sucesso = await postagemRepository.Alterar(postagem, cancellationToken);

        if (!sucesso)
            return Error.Failure("Postagem", "Erro ao alterar");

        return Result.Success;
    }
}
