namespace ControleVendasAPI.Models;

public class Sale
{
    public DateTime SalesDay { get; set; }
    public string? ClientName { get; set; }
    public int Quantity { get; set; }
    public double SalesPrice { get; set; }
}