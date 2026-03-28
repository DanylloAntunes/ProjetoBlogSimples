using BlogSimples.Common.Eventos;
using BlogSimples.Notificacao.Server.Hub;
using BlogSimples.Notificacao.Server.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace BlogSimples.Notificacao.Server;

public class NotificarClienteHandler(
    IHubContext<NotificacaoHub, INotificacaoClient> hubContext,
    ILogger<NotificarClienteHandler> logger) : INotificationHandler<NotificarEvent>
{
    public async Task Handle(NotificarEvent notification, CancellationToken cancellationToken)
    {
        var notificacao = new NotificacaoDto(
            Guid.NewGuid(),
            notification.Titulo,
            notification.Mensagem,
            DateTime.UtcNow);

        await hubContext.Clients.All.ReceberNotificacao(notificacao);

        logger.LogInformation("Notificação enviada para todos: {Titulo}", notificacao.Titulo);
    }
}
