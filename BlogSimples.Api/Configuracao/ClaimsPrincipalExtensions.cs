using System.Security.Claims;

namespace BlogSimples.Api.Configuracao;

public static class ClaimsPrincipalExtensions
{
    public static Guid? ObterIdUsuario(this ClaimsPrincipal user)
    {
        var claim = user.FindFirst(ClaimTypes.NameIdentifier);

        return Guid.TryParse(claim?.Value, out var id)
            ? id
            : null;
    }

    public static string ObterNomeUsuario(this ClaimsPrincipal user)
    {
        var claim = user.FindFirst(ClaimTypes.Name);

        return claim is null ? string.Empty : claim.Value;
    }
}
