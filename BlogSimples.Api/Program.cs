using BlogSimples.Api.Configuracao;
using BlogSimples.Api.Controllers;
using BlogSimples.Api.Middlewares;
using BlogSimples.IoC;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure(builder.Configuration);

var app = builder.Build();

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI(option =>
{
    option.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog Simples Swagger Docs");
    option.RoutePrefix = "swagger";
    option.DisplayRequestDuration();
    option.DocExpansion(DocExpansion.None);
    option.EnableDeepLinking();
    option.ShowExtensions();
    option.ShowCommonExtensions();
});

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication(); 

app.UseAuthorization();  

app.UseMiddleware<GlobalExceptionHandler>();

using (var scope = app.Services.CreateScope())
{
    await MigracaoBD.ExecuteAsync(scope.ServiceProvider);
}

app.MapEndpointsUsuario();
app.MapEndpointsAutenticar();
app.MapEndpointsPostagem();

app.UseInfrastructure();

await app.RunAsync();

public partial class Program { }