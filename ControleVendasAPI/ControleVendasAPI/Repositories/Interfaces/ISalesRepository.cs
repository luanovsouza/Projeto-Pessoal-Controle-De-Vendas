using ControleVendasAPI.Models;
using ControleVendasAPI.Models.DTOS;
using Microsoft.AspNetCore.Mvc;

namespace ControleVendasAPI.Repositories.Interfaces;

public interface ISalesRepository
{
    Task<IEnumerable<Sale>> GetSales();
    
    Task<Sale> GetSaleById(int id);
    
    Task<Sale> PostSale(CreatedSaleDto dto);
    
    Task<Sale> PutSale(Sale sale);
    
    Task<Sale> DeleteSale(int id);
}