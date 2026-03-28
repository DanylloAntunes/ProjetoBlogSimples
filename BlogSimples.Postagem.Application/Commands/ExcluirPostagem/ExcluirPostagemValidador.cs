using FluentValidation;

namespace BlogSimples.Postagem.Application.Commands.ExcluirPostagem;

public class ExcluirPostagemValidador : AbstractValidator<ExcluirPostagemCommand>
{
    public ExcluirPostagemValidador()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id é obrigatório");

        RuleFor(x => x.IdUsuarioLogado)
            .NotEmpty()
            .WithMessage("Usuário inválido");
    }
}