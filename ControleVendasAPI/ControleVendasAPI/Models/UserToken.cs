using Microsoft.AspNetCore.Identity;

namespace ControleVendasAPI.Models;

public class UserToken : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime Expiracao { get; set; }
}