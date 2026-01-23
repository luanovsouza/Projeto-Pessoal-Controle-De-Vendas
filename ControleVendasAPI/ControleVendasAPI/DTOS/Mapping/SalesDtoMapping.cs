using AutoMapper;
using ControleVendasAPI.Models;

namespace ControleVendasAPI.DTOS.Mapping;

public class SalesDtoMapping : Profile
{
    public SalesDtoMapping()
    {
        CreateMap<Sale, SaleDto>().ReverseMap();
    }
}