using System.ComponentModel.DataAnnotations;

namespace ControleVendasAPI.Models;

public class Cost
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(30, ErrorMessage = "O nome deve conter entre 5 a 30 caracteres", MinimumLength = 5)]
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "O valor do gasto é obrigatorio")]
    public double Spent { get; set; } //Gasto
}