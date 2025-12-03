using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ControleVendasAPI.Models;

public class Sale
{
    [JsonIgnore]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "A data é obrigatória!")]
    [DataType(DataType.Date)]
    public DateTime SalesDay { get; set; }

    [Required]
    [StringLength(30, ErrorMessage = "O tipo do kit deve contem entre 5 a 30 caracteres", MinimumLength = 5)]
    public string? KitType { get; set; }
    
    [StringLength(30, ErrorMessage = "O nome do cliete deve conter entre 5 a 30 caracteres", MinimumLength = 5)]
    public string? ClientName { get; set; }
    
    [Required]
    [Range(1, 200)]
    public int Quantity { get; set; }
    
    [Required]
    [Range(5, 1000)]
    public double SalesPrice { get; set; }
}