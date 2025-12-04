using System.Collections.ObjectModel;
using Microsoft.VisualBasic;

namespace ControleVendasAPI.Models.DTOS;

public class CreatedSaleDto
{
    public int Id { get; set; }
    public DateTime SalesDay { get; set; }
    public string? ClientName { get; set; }
    public int Quantity { get; set; }
    public double SalesPrice { get; set; }
    public ICollection<int>? SweetKitsIds { get; set; } = new List<int>();
}