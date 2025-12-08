using ControleVendasAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleVendasAPI.Repositories.Interfaces;

public interface ISalesRepository
{
    Task<IEnumerable<Sale>> GetSales();
    
    Task<Sale> GetSaleById(int id);
    
    Task<Sale> PostSale(Sale sale);
    
    Task<Sale> PutSale(int id, Sale sale);
    
    Task<Sale> DeleteSale(int id);
}