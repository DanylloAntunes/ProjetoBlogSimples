namespace BlogSimples.Notificacao.Server.Interfaces;

public interface INotificacaoService
{
    Task EnviarParaTodosAsync(NotificacaoRequest request);
}
