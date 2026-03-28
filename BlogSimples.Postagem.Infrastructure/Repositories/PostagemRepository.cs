using BlogSimples.Common.Application;
using BlogSimples.Postagem.Application.Interfaces;
using BlogSimples.Postagem.Application.Queries.ObterPostagens;
using BlogSimples.Postagem.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace BlogSimples.Postagem.Infrastructure.Repositories;

public class PostagemRepository(PostagemDbContext contexto) : IPostagemRepository
{
    public async Task<string> Registrar(Domain.Postagem postagem, CancellationToken cancellationToken)
    {
        contexto.Postagem.Add(postagem);

        var resultado = await contexto.SaveChangesAsync(cancellationToken);

        return resultado > 0 ? postagem.Id.ToString() : string.Empty;
    }

    public async Task<bool> Alterar(Domain.Postagem postagem, CancellationToken cancellationToken)
    {
        contexto.Postagem.Update(postagem);

        var resultado = await contexto.SaveChangesAsync(cancellationToken);

        return resultado > 0;
    }

    public async Task<bool> Excluir(Domain.Postagem postagem, CancellationToken cancellationToken)
    {
        contexto.Postagem.Remove(postagem);

        var resultado = await contexto.SaveChangesAsync(cancellationToken);

        return resultado > 0;
    }

    public Task<bool> Existe(string id, CancellationToken cancellationToken)
    {
        return contexto.Postagem.AnyAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Domain.Postagem?> Obter(string id, CancellationToken cancellationToken)
    {
        return contexto.Postagem.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PaginadoResponse<ObterPostagensResponse>> ObterPostagensPaginada(DateTime? ultimaDataCriacao, string? ultimoId, int totalRegistroPagina, CancellationToken cancellationToken)
    {
        totalRegistroPagina = Math.Min(totalRegistroPagina, 100);

        IQueryable<Domain.Postagem> query = contexto.Postagem
            .AsNoTracking()
            .OrderByDescending(x => x.DataCriacao)
            .ThenByDescending(x => x.Id);

        if (ultimaDataCriacao.HasValue && !string.IsNullOrEmpty(ultimoId))
        {
            query = query.Where(x =>
                x.DataCriacao < ultimaDataCriacao.Value ||
                (x.DataCriacao == ultimaDataCriacao.Value && string.Compare(x.Id, ultimoId) < 0));
        }

        var postagens = await query
            .Take(totalRegistroPagina + 1)
            .Select(x => new ObterPostagensResponse(x.Id, x.Titulo, x.Conteudo, x.DataCriacao))
            .ToListAsync(cancellationToken);

        var possuiProximaPagina = postagens.Count > totalRegistroPagina;

        if (possuiProximaPagina)
            postagens.RemoveAt(postagens.Count - 1);

        if (possuiProximaPagina && postagens.Count > 0)
        {
            var lastItem = postagens.Last();

            ultimaDataCriacao = lastItem.DataCriacao;
            ultimoId = lastItem.Id;
        }

        return new PaginadoResponse<ObterPostagensResponse>(
            postagens,
            ultimaDataCriacao,
            ultimoId,
            possuiProximaPagina
        );
    }
}
