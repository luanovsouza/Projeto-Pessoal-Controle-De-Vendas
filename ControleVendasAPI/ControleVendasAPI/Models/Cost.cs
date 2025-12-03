using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ControleVendasAPI.Models;

public class Cost
{
    [JsonIgnore]
    public int Id { get; set; }
    
    [Required]
    [StringLength(30, ErrorMessage = "O nome deve conter entre 5 a 30 caracteres", MinimumLength = 5)]
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "O valor do gasto é obrigatório")]
    public double Spent { get; set; } //Gasto
}