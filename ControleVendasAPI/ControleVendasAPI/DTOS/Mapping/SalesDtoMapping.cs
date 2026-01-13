using AutoMapper;
using ControleVendasAPI.Models;
using ControleVendasAPI.Models.DTOS;

namespace ControleVendasAPI.DTOS.Mapping;

public class SalesDtoMapping : Profile
{
    public SalesDtoMapping()
    {
        CreateMap<Sale, SaleDto>().ReverseMap();
    }
}