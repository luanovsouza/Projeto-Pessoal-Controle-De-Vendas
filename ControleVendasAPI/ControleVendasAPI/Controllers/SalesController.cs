using ControleVendasAPI.Context;
using ControleVendasAPI.Models;
using ControleVendasAPI.Models.DTOS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ControleVendasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly AppDbContext _context;

    public SalesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CreatedSaleDto>>> GetSales()
    {
        var sales = await _context.Sales.ToListAsync();

        if (sales == null)
        {
            return NotFound("No sale was found!");
        }
        
        var sale = _context.Sales.Include(x => x.SweetKits).ToList();
        
        var salesDto = sales.Select(dto => new CreatedSaleDto
        {
            Id = dto.Id,
            SalesDay = dto.SalesDay,
            ClientName = dto.ClientName,
            Quantity = dto.Quantity,
            SalesPrice = dto.SalesPrice,
            SweetKitsIds = dto.SweetKits.Select(k => k.Id).ToList(), // Vou mostrar todos os IDs do SweetKits da venda
        }).ToList();
        
        return Ok(salesDto);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetSale")]
    public async Task<ActionResult<Sale>> SaleById(int id)
    {
        var saleById = await _context.Sales.FirstOrDefaultAsync(s => s.Id == id);

        if (saleById == null)
            return NotFound("This sale was not found.");

        return Ok(saleById);
    }

    [HttpPost]
    public async Task<ActionResult<SweetKit>> PostSale(CreatedSaleDto dto)
    {
        var kits = await _context.SweetKits.Where(k => 
            dto.SweetKitsIds!.Contains(k.Id)).ToListAsync();

        if (!kits.Any())
            return NotFound("No sweet kits were found.");
        
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
        return new CreatedAtRouteResult("GetSale", new { id = sales.Id }, sales);

    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult<Sale>> PutSale(int id, Sale? sale)
    {
        var saleById = await _context.Sales.FirstOrDefaultAsync(s => s.Id == id);

        if (saleById == null)
        {
            return NotFound($"Sale {id} was not found.");
        }

        _context.Entry(sale).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Ok(sale);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult<Sale>> DeleteSale(int id)
    {
        var saleById = await _context.Sales.FirstOrDefaultAsync(s => s.Id == id);
        
        if (saleById == null)
            return  NotFound($"Sale {id} was not found.");
        
        _context.Sales.Remove(saleById);
        await _context.SaveChangesAsync();

        return Ok(saleById);
    }
}