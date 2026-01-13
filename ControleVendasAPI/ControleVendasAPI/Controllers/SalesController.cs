using AutoMapper;
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
    private readonly IMapper _mapper;

    public SalesController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<SaleDto>> GetSales()
    {
        var sales = _uof.SalesRepository.GetAll();

        if (sales == null)
        {
            return NotFound("No sale was found!");
        }
        
        var salesDto = _mapper.Map<IEnumerable<SaleDto>>(sales);
        
        return Ok(salesDto);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetSale")]
    public ActionResult<Sale> SaleById(int id)
    {
        var saleById = _uof.SalesRepository.GetById(c => c.Id == id);

        if (saleById == null)
            return NotFound("This sale was not found.");
        
        var saleByIdDto = _mapper.Map<SaleDto>(saleById);

        return Ok(saleByIdDto);
    }

    [HttpPost]
    public async Task<ActionResult<SaleDto>> PostSale(SaleDto? saleDto)
    {
        if (saleDto == null)
            return BadRequest("Invalid data");
        
        var sale = _mapper.Map<Sale>(saleDto);
        
        var saleCreated = _uof.SalesRepository.Create(sale);
        await _uof.Commit();
        
        var saleDtoCreated = _mapper.Map<SaleDto>(saleCreated);
        
        return new CreatedAtRouteResult("GetSale", new { id = saleDtoCreated.Id }, saleDtoCreated);
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult<Sale>> PutSale(int id, SaleDto? saleDto)
    {
        var saleById = _uof.SalesRepository.GetById(c => c.Id == id);;

        if (saleById == null)
        {
            return NotFound($"Sale {id} was not found.");
        }
        
        var sale = _mapper.Map<Sale>(saleDto);
        
        _uof.SalesRepository.Update(saleById);
        await _uof.Commit();
        
        var saleDtoUpdated = _mapper.Map<SaleDto>(sale);
        
        return Ok(saleDtoUpdated);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult<Sale>> DeleteSale(int id)
    {
        var saleById = _uof.SalesRepository.GetById(c => c.Id == id);
        
        if (saleById == null)
            return  NotFound($"Sale {id} was not found.");
        
        _uof.SalesRepository.Delete(saleById);
        await _uof.Commit();
        
        var saleDeletedDto = _mapper.Map<SaleDto>(saleById);

        return Ok(saleDeletedDto);
    }
}