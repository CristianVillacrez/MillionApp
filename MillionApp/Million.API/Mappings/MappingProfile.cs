using AutoMapper;
using Million.Application.DTOs;
using MongoDB.Bson;

namespace Million.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BsonDocument, PropertyListItemDto>()
                .ForMember(dest => dest.IdProperty, opt => opt.MapFrom(src => src.GetValue("IdProperty").ToString()))
                .ForMember(dest => dest.PropertyName, opt => opt.MapFrom(src => src.GetValue("Name").AsString))
                .ForMember(dest => dest.PropertyAddress, opt => opt.MapFrom(src => src.GetValue("Address").AsString))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.GetValue("Price").ToDecimal()))
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Contains("OwnerName") ? src.GetValue("OwnerName").AsString : string.Empty))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Contains("Image") ? src.GetValue("Image").AsString : string.Empty))
                .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.GetValue("IdOwner").ToString()));
        }
    }
}
