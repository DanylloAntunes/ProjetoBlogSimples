using FluentValidation;

namespace BlogSimples.Autenticacao.Application.Commands.Login;

public class LoginValidador : AbstractValidator<LoginCommand>
{
    public LoginValidador()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório")
            .EmailAddress().WithMessage("Email ou senha inválidos")
            .MaximumLength(200);

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha é obrigatória")
            .MinimumLength(5).WithMessage("Email ou senha inválidos")
            .Matches("[A-Z]").WithMessage("Email ou senha inválidos")
            .Matches("[a-z]").WithMessage("Email ou senha inválidos")
            .Matches("[0-9]").WithMessage("Email ou senha inválidos")
            .Matches("[^a-zA-Z0-9]").WithMessage("Email ou senha inválidos");
    }
}
