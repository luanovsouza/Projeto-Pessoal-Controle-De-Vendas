using ControleVendasAPI.Context;
using ControleVendasAPI.Models;
using ControleVendasAPI.Models.DTOS;
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
        
        var sale = _context.Sales.Include(x => x.SweetKits).ToList();
        
        
        return sales;
    }

    public async Task<Sale> GetSaleById(int id)
    {
        var saleById = await _context.Sales.FirstOrDefaultAsync(s => s.Id == id);
        
        if(saleById == null)
            throw new KeyNotFoundException("Sale not found");

        return saleById;
    }

    public async Task<Sale> PostSale(CreatedSaleDto dto)
    {
        var kits = await _context.SweetKits.Where(k => 
            dto.SweetKitsIds!.Contains(k.Id)).ToListAsync();
        
        var sales = new Sale
        {
            SalesDay = dto.SalesDay,
            ClientName = dto.ClientName,
            Quantity = dto.Quantity,
            SalesPrice = dto.SalesPrice,                    
            SweetKits = kits!
        };
        
        _context.Sales.Add(sales);
        await _context.SaveChangesAsync();
        
        return sales;
    }

    public async Task<Sale> PutSale(Sale sale)
    {
        if(sale == null)
            throw new ArgumentNullException("Sale is null, write again");
        
        _context.Entry(sale).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        
        return sale;
    }

    public async Task<Sale> DeleteSale(int id)
    {
        var saleDeleted = await _context.Sales.FirstOrDefaultAsync(d => d.Id == id);
        
        if(saleDeleted == null)
            throw new ArgumentNullException((nameof(saleDeleted)));
        
        _context.Sales.Remove(saleDeleted);
        await _context.SaveChangesAsync();
        
        return saleDeleted;
    }
}