using BlogSimples.Postagem.Application.Interfaces;
using ErrorOr;
using MediatR;

namespace BlogSimples.Postagem.Application.Commands.ExcluirPostagem;

public class ExcluirPostagemCommandHandler(IPostagemRepository postagemRepository) : IRequestHandler<ExcluirPostagemCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(ExcluirPostagemCommand request, CancellationToken cancellationToken)
    {
        var postagem = await postagemRepository.Obter(request.Id, cancellationToken);

        if (postagem is null)
            return Error.NotFound("Postagem", "Postagem não encontrada");

        var validarExclusao = postagem.PodeExcluir(request.IdUsuarioLogado);

        if (validarExclusao.IsError)
            return validarExclusao.Errors;

        var sucesso = await postagemRepository.Excluir(postagem, cancellationToken);

        if (!sucesso)
            return Error.Failure("Postagem", "Erro ao excluir");

        return Result.Success;
    }
}
