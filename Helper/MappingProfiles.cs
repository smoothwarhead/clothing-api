using AutoMapper;
using KerryCoAdmin.Api.Entities.Dtos.Responses;
using KerryCoAdmin.Api.Entities.Models;
using KerryCoAdmin.Entities.Dtos.Response;
using KerryCoAdmin.Models;
using Microsoft.AspNetCore.Identity;

namespace KerryCoAdmin.Helper
{
    public class MappingProfiles : Profile
    {
        
        public MappingProfiles()
        {
            ;

            CreateMap<IdentityUser, UserResponse>().ReverseMap();

            CreateMap<Admin, AdminProfileResponse>().ReverseMap();

            CreateMap<Product, ProductResponse>().ReverseMap();

            CreateMap<ProductVariation, VariationResponse>().ReverseMap();





        }
    }
}
