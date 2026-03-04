using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ControleVendasAPI.DTOS;
using ControleVendasAPI.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace ControleVendasAPI.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenUserDto GerarToken(LoginDto loginDto)
    {
        try
        {

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, loginDto.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            
            var key = _config.GetSection("JWT").GetValue<string>("SecretKey");
        
            var chavePrivada = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
        
            var credentials = new SigningCredentials(chavePrivada, SecurityAlgorithms.HmacSha256);
            
            var expiracao = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config["JWT:ExpireInMinutes"]));
            try
            {
                var token = new JwtSecurityToken(
                    issuer: _config["JWT:ValidIssuer"],
                    audience: _config["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(30),
                    claims: claims,
                    signingCredentials: credentials
                );
            
                return new TokenUserDto()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
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
        catch (Exception e)
        {
            Console.WriteLine($"Ocorreu um erro inesperado, {e.Message}");
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
}