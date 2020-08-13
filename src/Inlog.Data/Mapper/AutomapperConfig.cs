using AutoMapper;
using Inlog.Domain.Dtos;
using Inlog.Domain.Entities;

namespace Inlog.Data.Mapper
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Veiculo, VeiculoDto>();
            CreateMap<VeiculoDto, Veiculo>();
            CreateMap<Veiculo, VeiculoDetalheDto>().ReverseMap(); 
            CreateMap<VeiculoDto, VeiculoDetalheDto>().ReverseMap();

        }
    }
}
