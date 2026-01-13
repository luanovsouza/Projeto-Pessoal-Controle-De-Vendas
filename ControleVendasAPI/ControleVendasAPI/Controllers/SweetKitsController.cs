using AutoMapper;
using ControleVendasAPI.Context;
using ControleVendasAPI.DTOS;
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
    private readonly IMapper _mapper;

    public SweetKitsController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<SweetKitDto>> GetSweetKits()
    {
        var kits = _uof.SweetKitRepository.GetAll();

        if (kits == null)
        {
            return NotFound("Kits não encontrados!");
        }
        
        var kitDto = _mapper.Map<IEnumerable<SweetKitDto>>(kits);
        return Ok(kitDto);
    }

    [HttpGet("{id:min(1)}", Name = "GetKit")]
    public ActionResult<SweetKitDto> GetSweetKit(int id)
    {
        var sweetKit = _uof.SweetKitRepository.GetById(s => s.Id == id);

        if (sweetKit == null)
        {
            return NotFound("Não foi encontrado este Kit!");
        }
        
        var kitDto = _mapper.Map<SweetKitDto>(sweetKit);
        
        return Ok(kitDto);
    }

    [HttpPost]
    public async Task<ActionResult<SweetKitDto>> PostSweetKit(SweetKitDto? sweetKitDto)
    {
        if (sweetKitDto == null)
            return BadRequest("Dados invalidos digite corrretamente");
        
        var kitCreated = _mapper.Map<SweetKit>(sweetKitDto);
        
        _uof.SweetKitRepository.Create(kitCreated); 
        await _uof.Commit();   
        
        var kitDto = _mapper.Map<SweetKitDto>(kitCreated);

        return new CreatedAtRouteResult("GetKit", new { id = kitDto.Id }, kitDto);
    }

    [HttpPut("{id:min(1)}")]
    public async Task<ActionResult<SweetKitDto>> PutSweetKit(int id, SweetKitDto? sweetKitDto)
    {
        if (sweetKitDto != null && id != sweetKitDto.Id)
        {
            return BadRequest("Dados invalidos digite corrtatamente");
        }
        
        var sweetKit = _mapper.Map<SweetKit>(sweetKitDto);

        _uof.SweetKitRepository.Update(sweetKit);
        await _uof.Commit();
        
        var sweekitUpdatedDto = _mapper.Map<SweetKitDto>(sweetKit);

        return Ok(sweekitUpdatedDto);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult<SweetKitDto>> DeleteSweetKit(int id)
    {
        var sweetKitDeleted = _uof.SweetKitRepository.GetById(s => s.Id == id);

        if (sweetKitDeleted == null)
        {
            return NotFound("Kit not found :(");
        }

        _uof.SweetKitRepository.Delete(sweetKitDeleted);
        await _uof.Commit();

        var sweekitDeletedDto = _mapper.Map<SweetKitDto>(sweetKitDeleted);

        return Ok(sweekitDeletedDto);
    }
}