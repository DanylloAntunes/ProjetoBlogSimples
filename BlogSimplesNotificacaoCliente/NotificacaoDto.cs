namespace BlogSimples.Notificacao.Cliente;

public record NotificacaoDto(
    Guid Id,
    string Titulo,
    string Mensagem,
    DateTime Timestamp);
