using BlogSimples.Notificacao.Cliente;

var cliente = new NotificacaoClient("http://localhost:5000/hubs/notificacao");

await cliente.IniciarAsync();

Console.WriteLine("Pressione ENTER para encerrar.");
Console.ReadLine();

await cliente.PararAsync();