namespace BlogSimples.Notificacao.Server.Interfaces;

public interface INotificacaoClient
{
    Task ReceberNotificacao(NotificacaoDto notificacao);
}
