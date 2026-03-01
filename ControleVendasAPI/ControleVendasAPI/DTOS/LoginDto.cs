using System.ComponentModel.DataAnnotations;

namespace ControleVendasAPI.DTOS;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string? Senha { get; set; }
}