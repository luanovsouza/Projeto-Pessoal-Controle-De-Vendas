namespace ControleVendasAPI.DTOS;

public class TokenUserDto
{
    public bool Autenticado { get; set; }
    public DateTime Expiracao { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
}