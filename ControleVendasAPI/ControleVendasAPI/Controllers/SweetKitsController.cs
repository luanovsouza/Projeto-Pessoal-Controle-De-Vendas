using ControleVendasAPI.Context;
using ControleVendasAPI.Models;
using ControleVendasAPI.Models.DTOS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ControleVendasAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class SweetKitsController : ControllerBase
{
    
    private readonly AppDbContext _context;

    public SweetKitsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SweetKit>>> GetSweetKits()
    {
        try
        {
            var kits = await _context.SweetKits.AsNoTracking().ToListAsync();

            if (kits == null)
            {
                return NotFound("Kits não encontrados!");
            }
        
            return Ok(kits);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,$"Ocorreu um erro ao criar o SweetKit {e.Message}");
        }
        
    }

    [HttpGet("{id:min(1)}", Name = "GetKit")]
    public async Task<ActionResult<SweetKit>> GetSweetKit(int id)
    {
        try
        {
            var sweetKit = await _context.SweetKits.FirstOrDefaultAsync(s => s.Id == id);

            if (sweetKit == null)
            {
                return  NotFound("Não foi encontrado este Kit!");
            }
        
            return Ok(sweetKit);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Ocorreu um erro ao buscar o SweetKit {e.Message}");
        }
        
    }

    [HttpPost]
    public async Task<ActionResult<SweetKit>> PostSweetKit(SweetKitDto? dto)
    {
        try
        {
            if (dto == null)
                return BadRequest("Dados invalidos digite corrretamente");
            
            var kitCreated = new SweetKit
            {
                Name = dto.Name,
                Quantity = dto.Quantity,
                KitPrice = dto.KitPrice
            };
            _context.SweetKits.Add(kitCreated);
            await _context.SaveChangesAsync();
            
            return new CreatedAtRouteResult("GetKit", new { id = kitCreated.Id }, kitCreated);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Ocorreu um erro ao criar o SweetKit: {e.Message}");
        }
        
    }
    
    [HttpPut("{id:min(1)}")]
    public async Task<IActionResult> PutSweetKit(int id, SweetKit? sweetKit)
    {
        if (sweetKit != null && id != sweetKit.Id)
        {
            return BadRequest("Dados invalidos digite corrtatamente");
        }
        
        _context.Entry(sweetKit).State = EntityState.Modified;
        
        await _context.SaveChangesAsync();
        
        return Ok(sweetKit);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult<SweetKit>> DeleteSweetKit(int id)
    {
        var sweetKitDeleted = await _context.SweetKits.FirstOrDefaultAsync(s => s.Id == id);

        if (sweetKitDeleted == null)
        {
            return NotFound("Kit not found :(");
        }
        
        _context.SweetKits.Remove(sweetKitDeleted);
        await _context.SaveChangesAsync();
        
        return Ok(sweetKitDeleted);
    }
}