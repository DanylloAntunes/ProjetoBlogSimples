using Microsoft.AspNetCore.SignalR.Client;

namespace BlogSimples.Notificacao.Cliente;

public class RetryPolicy : IRetryPolicy
{
    private static readonly TimeSpan[] _intervalos =
    [
        TimeSpan.Zero,
        TimeSpan.FromSeconds(2),
        TimeSpan.FromSeconds(5),
        TimeSpan.FromSeconds(15),
        TimeSpan.FromSeconds(30)
    ];

    public TimeSpan? NextRetryDelay(RetryContext retryContext)
    {
        var index = (int)retryContext.PreviousRetryCount;

        if (index >= _intervalos.Length)
            return null;

        Console.WriteLine($"[Reconexão] Tentativa {index + 1} em {_intervalos[index].TotalSeconds}s...");
        return _intervalos[index];
    }
}
