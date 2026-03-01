using ControleVendasAPI.Context;
using ControleVendasAPI.Repositories.Interfaces;
using ControleVendasAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ControleVendasAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RelatorioFinanceiroController : ControllerBase
{
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRelatorioFinanceiroDto _relatorioFinanceiroDto;
    
    public RelatorioFinanceiroController(IUnitOfWork unitOfWork, IRelatorioFinanceiroDto relatorioFinanceiroDto)
    {
        _unitOfWork = unitOfWork;
        _relatorioFinanceiroDto = relatorioFinanceiroDto;
    }

    [HttpGet("venda-dia")]
    public ActionResult GetRelatorioFinanceiro([FromQuery]int vendaId)
    {
        var buscarId = _unitOfWork.SalesRepository.GetById(bd => bd.Id == vendaId);

        if (vendaId != buscarId.Id )
        {
            return NotFound("Id nao foi encontrado!");
        }

        var resultado = _relatorioFinanceiroDto.RelatorioVendasDto(vendaId);
        
        return Ok(resultado);
    }
}