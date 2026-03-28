using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace BlogSimples.Notificacao.Cliente;


public class NotificacaoClient
{
    private readonly HubConnection _connection;

    public NotificacaoClient(string url)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect(new RetryPolicy())
            .ConfigureLogging(log => log.SetMinimumLevel(LogLevel.Warning))
            .Build();

        _connection.On<NotificacaoDto>("ReceberNotificacao", ExibirNotificacao);

        _connection.Reconnecting += _ =>
        {
            Console.WriteLine("[SignalR] Reconectando...");
            return Task.CompletedTask;
        };

        _connection.Reconnected += _ =>
        {
            Console.WriteLine("[SignalR] Reconectado.");
            return Task.CompletedTask;
        };

        _connection.Closed += erro =>
        {
            Console.WriteLine(erro is null
                ? "[SignalR] Conexão encerrada."
                : $"[SignalR] Encerrada com erro: {erro.Message}");
            return Task.CompletedTask;
        };
    }

    public async Task IniciarAsync()
    {
        await _connection.StartAsync();
        Console.WriteLine("[SignalR] Conectado. Aguardando notificações...\n");
    }

    public async Task PararAsync()
    {
        await _connection.StopAsync();
        await _connection.DisposeAsync();
    }

    private static void ExibirNotificacao(NotificacaoDto notificacao)
    {
        Console.WriteLine($"╔══ Nova publicação [{notificacao.Timestamp:HH:mm:ss}]");
        Console.WriteLine($"║  {notificacao.Titulo}");
        Console.WriteLine($"║  {notificacao.Mensagem}");

        Console.WriteLine("╚═══════════════════════════\n");
        Console.ResetColor();
    }
}
