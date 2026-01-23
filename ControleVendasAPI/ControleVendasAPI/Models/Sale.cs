using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ControleVendasAPI.Models;

public class Sale
{
    public Sale()
    {
        SweetKits = new Collection<SweetKit>();
    }

    [JsonIgnore]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "A data é obrigatória!")]
    [DataType(DataType.Date)]
    public DateTime SalesDay { get; set; }
    
    
    [StringLength(30, ErrorMessage = "O nome do cliete deve conter entre 5 a 30 caracteres", MinimumLength = 5)]
    public string? ClientName { get; set; }
    
    [Required]
    [Range(1, 200)]
    public int Quantity { get; set; }
    
    [Required]
    [Range(5, 1000)]
    public double SalesPrice { get; set; }
    
    [Required]
    public double SalePriceUnit { get; set; }
    
    //Financeiro
    public double Custo { get; set; }
    public double Faturamento { get; set; }
    public double Lucro { get; set; }
    
    //Relacionamento
    public ICollection<SweetKit> SweetKits { get; set; } = new  List<SweetKit>();
}