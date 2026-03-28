using FluentValidation;

namespace BlogSimples.Postagem.Application.Commands.RegistrarPostagem;

public class RegistrarPostagemValidador : AbstractValidator<RegistrarPostagemCommand>
{
    public RegistrarPostagemValidador()
    {
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
