using ErrorOr;
using Error = ErrorOr.Error;

namespace BlogSimples.Postagem.Domain;

public class Postagem
{
    public string Id { get; private set; }
    public string Titulo { get; private set; } = string.Empty;
    public string Conteudo { get; private set; } = string.Empty;
    public Guid AutorId { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public DateTime? DataAtualizacao { get; private set; }

    protected Postagem() { }

    public static ErrorOr<Postagem> Criar(
        string titulo,
        string conteudo,
        Guid usuarioLogado)
    {
        return new Postagem
        {
            Id = Ulid.NewUlid().ToString(),
            Titulo = titulo,
            Conteudo = conteudo,
            AutorId = usuarioLogado,
            DataCriacao = DateTime.UtcNow
        };
    }

    public ErrorOr<Success> Atualizar(
        string titulo,
        string conteudo,
        Guid usuarioLogado)
    {
        if (usuarioLogado != AutorId)
            return Error.Unauthorized("Postagem", "Usuário não pode alterar esta postagem"); 

        Titulo = titulo;
        Conteudo = conteudo;
        DataAtualizacao = DateTime.UtcNow;

        return Result.Success;
    }

    public ErrorOr<Success> PodeExcluir(Guid usuarioLogado)
    {
        if (usuarioLogado != AutorId)
            return Error.Unauthorized("Postagem", "Usuário não pode excluir esta postagem"); 

        return Result.Success;
    }
}