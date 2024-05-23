using Core.Tasks;
using AutoMapper;
using WebAPI.DTOs.Tasks;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TaskDto, TaskBody>();
            /*
            CreateMap<SubTaskDto, TaskSubBody>();
            CreateMap<CollaboratorDto, Collaborator>();
            CreateMap<WorkDayDto, WorkDay>();
            // Reverse mappings if needed
            CreateMap<TaskBody, TaskDto>();
            CreateMap<TaskSubBody, SubTaskDto>();
            CreateMap<Collaborator, CollaboratorDto>();
            CreateMap<WorkDay, WorkDayDto>();
            */
        }
    }
}
