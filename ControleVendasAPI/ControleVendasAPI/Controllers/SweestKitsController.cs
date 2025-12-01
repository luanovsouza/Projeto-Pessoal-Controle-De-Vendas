using ControleVendasAPI.Context;
using ControleVendasAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleVendasAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class SweestKitsController : ControllerBase
{
    
    private readonly AppDbContext _context;

    public SweestKitsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SweetKit>>> GetSweetKits()
    {
        var kits = await _context.SweetKits.AsNoTracking().ToListAsync();

        if (kits == null)
        {
            return NotFound("Kits não encontrados!");
        }
        
        return Ok(kits);
    }

    [HttpGet("{id:min(1)}")]
    public async Task<ActionResult<SweetKit>> GetSweetKit(int id)
    {
        var sweetKit = await _context.SweetKits.FirstOrDefaultAsync(s => s.Id == id);

        if (sweetKit == null)
        {
            return  NotFound("Não foi encontrado este Kit!");
        }
        
        return Ok(sweetKit);
    }
}