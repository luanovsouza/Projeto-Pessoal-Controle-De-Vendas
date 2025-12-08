using ControleVendasAPI.Context;
using ControleVendasAPI.Models;
using ControleVendasAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleVendasAPI.Repositories;

public class SaleRepository : ISalesRepository
{
    private readonly AppDbContext _context;

    public SaleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Sale>> GetSales()
    {
        var sales = await _context.Sales.ToListAsync();
        
        if(sales ==  null)
            throw new ArgumentNullException("Sales List is null");
        
        return sales;
    }

    public async Task<Sale> GetSaleById(int id)
    {
        var saleById = await _context.Sales.FirstOrDefaultAsync(s => s.Id == id);
        
        if(saleById == null)
            throw new KeyNotFoundException("Sale not found");

        return saleById;
    }

    public async Task<Sale> PostSale(Sale sale)
    {
        if(sale == null)
            throw new ArgumentNullException("Sale is null write again");
        
        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();
        
        return sale;
    }

    public Task<Sale> PutSale(int id, Sale sale)
    {
        throw new NotImplementedException();
    }

    public Task<Sale> DeleteSale(int id)
    {
        throw new NotImplementedException();
    }
}