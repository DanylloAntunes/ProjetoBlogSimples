using BlogSimples.Notificacao.Server.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace BlogSimples.Notificacao.Server.Hub;

public class NotificacaoHub(ILogger<NotificacaoHub> logger) : Hub<INotificacaoClient>
{
    public override Task OnConnectedAsync()
    {
        logger.LogInformation("Cliente conectado: {ConnectionId}", Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        logger.LogInformation("Cliente desconectado: {ConnectionId}", Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}
