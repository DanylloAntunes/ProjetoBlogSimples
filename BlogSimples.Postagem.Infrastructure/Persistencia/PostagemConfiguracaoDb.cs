using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogSimples.Postagem.Infrastructure.Persistencia;

public class PostagemConfiguracaoDb : IEntityTypeConfiguration<Domain.Postagem>
{
    public void Configure(EntityTypeBuilder<Domain.Postagem> builder)
    {
        builder.ToTable("postagens", "postagem");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id);

        builder.Property(x => x.Titulo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Conteudo)
            .IsRequired();

        builder.Property(x => x.AutorId)
            .IsRequired()
            .HasColumnType("uuid");

        builder.Property(x => x.DataCriacao)
            .IsRequired();

        builder.Property(x => x.DataAtualizacao);
    }
}