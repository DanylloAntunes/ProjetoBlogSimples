using BlogSimples.Autenticacao.Application.Interfaces;
using BlogSimples.Autenticacao.Application.Settings;
using BlogSimples.Autenticacao.Domain;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogSimples.Autenticacao.Infrastructure.TokenService;

public class JwtTokenGeneratorServices(IOptions<JwtSettings> options) : ITokenGeneratorServices
{
    public string ObterToken(Usuario usuario)
    {
        var jwtSetting = options.Value;
        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Key));
        var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email)
        };

        var token = new JwtSecurityToken(
            issuer: jwtSetting.Issuer,
            audience: jwtSetting.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtSetting.ExpiresInMinutes),
            signingCredentials: credenciais);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
