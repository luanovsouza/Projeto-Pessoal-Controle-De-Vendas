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
    
    
    [Required]
    [Range(1, 200)]
    public int Quantity { get; set; }
    
    [Required]
    [Range(5, 1000)]
    public decimal SalePriceUnit { get; set; }
    
    
    //Relacionamento
    public ICollection<SweetKit> SweetKits { get; set; } = new  List<SweetKit>();
}