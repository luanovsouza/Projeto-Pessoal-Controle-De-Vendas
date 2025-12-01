using System.ComponentModel.DataAnnotations;

namespace ControleVendasAPI.Models;

public class Sale
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "A data é obrigatória!")]
    [DataType(DataType.Date)]
    public DateTime SalesDay { get; set; }
    
    [Required]
    [StringLength(30, ErrorMessage = "O nome do cliete deve conter entre 5 a 30 caracteres", MinimumLength = 5)]
    public string? ClientName { get; set; }
    
    [Required]
    [Range(1, 200)]
    public int Quantity { get; set; }
    
    [Required]
    [Range(5, 1000)]
    public double SalesPrice { get; set; }
}