using AutoMapper;
using DishesAPI.Entities;
using DishesAPI.Models;


namespace DishesAPI.Profiles;
public class DishProfile : Profile
{
    public DishProfile()
    {
        CreateMap<Dish, DishDTO>();
        CreateMap<DishDTO, Dish>();
    }
}
