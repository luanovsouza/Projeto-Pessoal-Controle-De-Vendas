using System.Security.Claims;
using ControleVendasAPI.DTOS;
using ControleVendasAPI.Models;

namespace ControleVendasAPI.Services.Interfaces;

public interface ITokenService
{
    public Task<TokenUserDto> GerarToken(UserToken user);
    public string GerarRefreshToken();
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    
}