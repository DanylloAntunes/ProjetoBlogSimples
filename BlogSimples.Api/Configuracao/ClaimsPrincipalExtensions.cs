using System.Security.Claims;

namespace BlogSimples.Api.Configuracao;

public static class ClaimsPrincipalExtensions
{
    public static Guid? GetUserId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirst(ClaimTypes.NameIdentifier);

        return Guid.TryParse(claim?.Value, out var id)
            ? id
            : null;
    }
}
