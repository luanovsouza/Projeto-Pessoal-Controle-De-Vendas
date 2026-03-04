using System.Security.Claims;
using ControleVendasAPI.DTOS;

namespace ControleVendasAPI.Services.Interfaces;

public interface ITokenService
{
    public TokenUserDto GerarToken(LoginDto loginDto);
    public string GerarRefreshToken();
}