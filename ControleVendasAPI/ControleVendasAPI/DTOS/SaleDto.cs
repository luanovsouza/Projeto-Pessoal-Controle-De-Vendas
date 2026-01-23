using System.Text.Json.Serialization;

namespace ControleVendasAPI.DTOS;

public class SaleDto 
{
    public int Id { get; set; }
    public DateTime SalesDay { get; set; }
    [JsonIgnore]
    public string? ClientName { get; set; }
    public int Quantity { get; set; } // Quantidade de Kits vendidos
    
    public double PrecoVendaUnitario { get; set; }
    public double SalesPrice { get; set; }
    public ICollection<int>? SweetKitsIds { get; set; } = new List<int>();
}