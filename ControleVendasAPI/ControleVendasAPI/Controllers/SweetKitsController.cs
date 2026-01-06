using ControleVendasAPI.Context;
using ControleVendasAPI.Models;
using ControleVendasAPI.Models.DTOS;
using ControleVendasAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ControleVendasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SweetKitsController : ControllerBase
{
    private readonly IUnitOfWork _uof;

    public SweetKitsController(IUnitOfWork uof)
    {
        _uof = uof;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SweetKit>>> GetSweetKits()
    {
        var kits = _uof.SweetKitRepository.GetAll();

        if (kits == null)
        {
            return NotFound("Kits não encontrados!");
        }

        return Ok(kits);
    }

    [HttpGet("{id:min(1)}", Name = "GetKit")]
    public ActionResult<SweetKit> GetSweetKit(int id)
    {
        var sweetKit = _uof.SweetKitRepository.GetById(s => s.Id == id);

        if (sweetKit == null)
        {
            return NotFound("Não foi encontrado este Kit!");
        }

        return Ok(sweetKit);
    }

    [HttpPost]
    public ActionResult<SweetKit> PostSweetKit(SweetKitDto? dto)
    {
        if (dto == null)
            return BadRequest("Dados invalidos digite corrretamente");

        var kitCreated = new SweetKit
        {
            Name = dto.Name,
            Quantity = dto.Quantity,
            KitPrice = dto.KitPrice
        };
        
        _uof.SweetKitRepository.Create(kitCreated); 
        _uof.Commit();   

        return new CreatedAtRouteResult("GetKit", new { id = kitCreated.Id }, kitCreated);
    }

    [HttpPut("{id:min(1)}")]
    public IActionResult PutSweetKit(int id, SweetKit? sweetKit)
    {
        if (sweetKit != null && id != sweetKit.Id)
        {
            return BadRequest("Dados invalidos digite corrtatamente");
        }

        _uof.SweetKitRepository.Update(sweetKit);
        _uof.Commit();

        return Ok(sweetKit);
    }

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult<SweetKit> DeleteSweetKit(int id)
    {
        var sweetKitDeleted = _uof.SweetKitRepository.GetById(s => s.Id == id);

        if (sweetKitDeleted == null)
        {
            return NotFound("Kit not found :(");
        }

        _uof.SweetKitRepository.Delete(sweetKitDeleted);
        _uof.Commit();

        return Ok(sweetKitDeleted);
    }
}