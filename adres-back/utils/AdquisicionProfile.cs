using adres.Models;
using adres.Models.Dto;
using AutoMapper;

public class AdquisicionProfile : Profile
{
    public AdquisicionProfile()
    {
        CreateMap<AdquisicionDto, Adquisicion>();
        CreateMap<Adquisicion, AdquisicionDto>();
    }
}