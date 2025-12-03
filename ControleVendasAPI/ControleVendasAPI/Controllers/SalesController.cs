using ControleVendasAPI.Context;
using ControleVendasAPI.Models;
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
    public async Task<ActionResult<IEnumerable<Sale>>> GetSales()
    {
        var sales = await _context.Sales.ToListAsync();

        if (sales == null)
        {
            return NotFound("No sale was found!");
        }

        return Ok(sales);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<Sale>> SaleById(int id)
    {
        var saleById = await _context.Sales.FirstOrDefaultAsync(s => s.Id == id);

        if (saleById == null)
            return NotFound("This sale was not found.");

        return Ok(saleById);
    }

    [HttpPost]
    public async Task<ActionResult<Sale>> PostSale(Sale? sale)
    {
        if (sale == null)
        {
            return BadRequest("Invalid data, please try again.");
        }
        
        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();
        
        return new CreatedAtRouteResult("GetSale", new { id = sale.Id }, sale); // retorna 201
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