namespace BlogSimples.Notificacao.Server;

public record NotificacaoDto(
    Guid Id,
    string Titulo,
    string Mensagem,
    DateTime Timestamp);