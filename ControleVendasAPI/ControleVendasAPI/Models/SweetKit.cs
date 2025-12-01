using System.ComponentModel.DataAnnotations;

namespace ControleVendasAPI.Models;

public class SweetKit
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(30, ErrorMessage = "O nome deve conter entre 5 a 30 caracteres", MinimumLength = 5)]
    public string? Name { get; set; }
    
    [Required]
    [Range(1, 1000)]
    public int Quantity { get; set; }
    
    [Required]
    [Range(5, 1000)]
    public double KitPrice { get; set; }
}