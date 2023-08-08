using AutoMapper;
using PizzeriaApp.Models;
using PizzeriaApp.ViewModels;

namespace PizzeriaApp.Configurations
{
    public class InitalizeMapper : Profile
    {
        public InitalizeMapper()
        {
            CreateMap<Product, ViewProduct>().ReverseMap();
        }
    }
}
