using AutoMapper;
using FinancialSupport.Application.DTOs;
using FinancialSupport.Domain.Entities;

namespace FinancialSupport.Application.Mappings
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
            CreateMap<Emprestimo, EmprestimoDTO>().ReverseMap();
            CreateMap<Parcela, ParcelaDTO>().ReverseMap();
        }
    }
}
