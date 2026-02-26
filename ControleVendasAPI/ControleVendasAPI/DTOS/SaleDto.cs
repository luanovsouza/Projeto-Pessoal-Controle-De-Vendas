using System.Text.Json.Serialization;

namespace ControleVendasAPI.DTOS;

public class SaleDto 
{
    public int Id { get; set; }
    
    public DateTime SalesDay { get; set; }
    
    public int Quantity { get; set; } // Quantidade de Kits vendidos
    
    public decimal SalePriceUnit { get; set; }
    
    public ICollection<int>? SweetKitsIds { get; set; } = new List<int>();
    
}