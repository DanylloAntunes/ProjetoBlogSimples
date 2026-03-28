namespace BlogSimples.Autenticacao.Domain;

public class Usuario
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Nome { get; set; }
    public required string Email { get; set; }
    public string Senha { get; private set; } = string.Empty;

    public void DefinirSenha(string senha)
    {
        Senha = senha;
    }
}
