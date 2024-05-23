using Core.Tasks;
using AutoMapper;
using Application.DTOs.Tasks;
using Application.DTOs.Users;
using Core.Users;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TaskDto, TaskBody>();
            CreateMap<RegisterUserModel, User>();
        }
    }
}
