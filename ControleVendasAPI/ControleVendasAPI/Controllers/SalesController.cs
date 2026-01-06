using ControleVendasAPI.Context;
using ControleVendasAPI.Models;
using ControleVendasAPI.Models.DTOS;
using ControleVendasAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ControleVendasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly IUnitOfWork _uof;

    public SalesController(IUnitOfWork uof)
    {
        _uof = uof;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CreatedSaleDto>>> GetSales()
    {
        var sales = _uof.SalesRepository.GetAll();

        if (sales == null)
        {
            return NotFound("No sale was found!");
        }
        
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
        var saleById = _uof.SalesRepository.GetById(c => c.Id == id);

        if (saleById == null)
            return NotFound("This sale was not found.");

        return Ok(saleById);
    }

    [HttpPost]
    public async Task<ActionResult<SweetKit>> PostSale(Sale sale)
    {
        if (sale == null)
            return BadRequest("Invalid data");
        
        var saleCreated = _uof.SalesRepository.Create(sale);
        _uof.Commit();
        
        return new CreatedAtRouteResult("GetSale", new { id = saleCreated.Id }, saleCreated);

    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult<Sale>> PutSale(int id, Sale? sale)
    {
        var saleById = _uof.SalesRepository.GetById(c => c.Id == id);;

        if (saleById == null)
        {
            return NotFound($"Sale {id} was not found.");
        }
        
        _uof.SalesRepository.Update(saleById);
        _uof.Commit();
        
        return Ok(sale);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult<Sale>> DeleteSale(int id)
    {
        var saleById = _uof.SalesRepository.GetById(c => c.Id == id);
        
        if (saleById == null)
            return  NotFound($"Sale {id} was not found.");
        
        _uof.SalesRepository.Delete(saleById);
        _uof.Commit();

        return Ok(saleById);
    }
}