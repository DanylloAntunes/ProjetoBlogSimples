using FluentValidation;

namespace BlogSimples.Autenticacao.Application.Commands.RegistrarUsuario;

public class RegistrarUsuarioValidador : AbstractValidator<RegistrarUsuarioCommand>
{
    public RegistrarUsuarioValidador()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório")
            .EmailAddress().WithMessage("Email inválido")
            .MaximumLength(200);

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MinimumLength(3).WithMessage("Nome deve ter no mínimo 3 caracteres")
            .MaximumLength(150);

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha é obrigatória")
            .MinimumLength(5).WithMessage("Senha deve ter no mínimo 5 caracteres")
            .Matches("[A-Z]").WithMessage("Senha deve conter letra maiúscula")
            .Matches("[a-z]").WithMessage("Senha deve conter letra minúscula")
            .Matches("[0-9]").WithMessage("Senha deve conter número")
            .Matches("[^a-zA-Z0-9]").WithMessage("Senha deve conter caractere especial");
    }
}
