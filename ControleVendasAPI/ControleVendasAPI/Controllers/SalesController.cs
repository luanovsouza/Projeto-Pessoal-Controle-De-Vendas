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
    private readonly ISalesRepository _repository;

    public SalesController(ISalesRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CreatedSaleDto>>> GetSales()
    {
        var sales = await _repository.GetSales();

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
        var saleById = await _repository.GetSaleById(id);

        if (saleById == null)
            return NotFound("This sale was not found.");

        return Ok(saleById);
    }

    [HttpPost]
    public async Task<ActionResult<SweetKit>> PostSale(CreatedSaleDto dto)
    {
        if (dto == null)
            return BadRequest("Invalid data");
        
        var saleCreated = await _repository.PostSale(dto);
        
        
        return new CreatedAtRouteResult("GetSale", new { id = saleCreated.Id }, saleCreated);

    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult<Sale>> PutSale(int id, Sale? sale)
    {
        var saleById = await _repository.GetSaleById(id);

        if (saleById == null)
        {
            return NotFound($"Sale {id} was not found.");
        }

        await _repository.PutSale(saleById);
        
        return Ok(sale);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult<Sale>> DeleteSale(int id)
    {
        var saleById = _repository.GetSaleById(id);
        
        if (saleById == null)
            return  NotFound($"Sale {id} was not found.");
        
        await _repository.DeleteSale(id);

        return Ok(saleById);
    }
}