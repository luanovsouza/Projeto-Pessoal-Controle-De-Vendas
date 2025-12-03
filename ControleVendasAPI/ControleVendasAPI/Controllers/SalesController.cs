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
}