using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ControleVendasAPI.DTOS;
using ControleVendasAPI.Models;
using ControleVendasAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ControleVendasAPI.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly UserManager<UserToken> _userManager;

    public TokenService(IConfiguration config, UserManager<UserToken> userManager)
    {
        _config = config;
        _userManager = userManager;
    }

    
    public async Task<TokenUserDto> GerarToken(UserToken user)
    {
        try
        {
            var key = _config["JWT:SecretKey"];
            if (string.IsNullOrEmpty(key))
                throw new InvalidOperationException("JWT:SecretKey não configurada");
            
            var roles = await _userManager.GetRolesAsync(user);
            //Obtendo as roles do usuario
            
            //Lista de claims pq o usuario pode ter varias roles
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            // Adicionando as roles como claims para o token
            
            var chavePrivada = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
            var credentials = new SigningCredentials(chavePrivada, SecurityAlgorithms.HmacSha256);
            
            var expiracao = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config["JWT:ExpireInMinutes"]));

            var token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                expires: expiracao,
                claims: claims,
                signingCredentials: credentials
            );
            
            var refreshToken = GerarRefreshToken();
            
            // Salvar refresh token no usuário
            user.RefreshToken = refreshToken;
            user.Expiracao = expiracao;
            

            
            return new TokenUserDto()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiracao = expiracao,
                Autenticado = true
            };
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao gerar o token {e.Message}");
            throw;
        }
    }
    
    public string GerarRefreshToken()
    {
        var bytes = new byte[128];
        
        using var bytesAleatorios = RandomNumberGenerator.Create();
        
        bytesAleatorios.GetBytes(bytes);
        var token = Convert.ToBase64String(bytes);
        
        return token;
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var secretKey = _config["JWT:SecretKey"];
        
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),
            ValidateLifetime = false
        };
        
        //ValidateAudience = false: Não valida se o público (audience) do token corresponde ao esperado.
        // ValidateIssuer = false: Não valida se o emissor (issuer) do token é o correto.
        // ValidateIssuerSigningKey = true: Valida se a chave de assinatura do emissor é válida.
        // IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)): Define a chave simétrica usada para verificar a assinatura do token.
        // ValidateLifetime = false: Não valida se o token expirou (útil para tokens expirados, como no refresh).
        
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Token inválido");

        return principal;
    }
}