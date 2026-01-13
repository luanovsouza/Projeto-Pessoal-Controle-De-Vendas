using AutoMapper;
using ControleVendasAPI.Models;

namespace ControleVendasAPI.DTOS.Mapping;

public class SweetKitDtoMapping : Profile
{
    public SweetKitDtoMapping()
    {
        CreateMap<SweetKit, SweetKitDto>().ReverseMap();
    }
}