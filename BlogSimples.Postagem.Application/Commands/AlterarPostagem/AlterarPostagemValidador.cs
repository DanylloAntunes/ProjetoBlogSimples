using FluentValidation;

namespace BlogSimples.Postagem.Application.Commands.AlterarPostagem;

public class AlterarPostagemValidador : AbstractValidator<AlterarPostagemCommand>
{
    public AlterarPostagemValidador()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id inválido");

        RuleFor(x => x.Titulo)
            .NotEmpty()
            .WithMessage("Título é obrigatório")
            .MaximumLength(200)
            .WithMessage("Título deve ter no máximo 200 caracteres");

        RuleFor(x => x.Conteudo)
            .NotEmpty()
            .WithMessage("Conteúdo é obrigatório");

        RuleFor(x => x.IdUsuarioLogado)
            .NotEmpty()
            .WithMessage("Usuário inválido");
    }
}