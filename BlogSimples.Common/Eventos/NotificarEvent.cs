using MediatR;

namespace BlogSimples.Common.Eventos;

public record NotificarEvent(string Titulo, string Mensagem) : INotification;

