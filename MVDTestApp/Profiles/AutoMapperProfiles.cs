using AutoMapper;

namespace MVDTestApp.Profiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<MVDTestApp.Data.Entityes.WorkTask, MVDTestApp.Model.WorkTask>()
                .ForMember(x=>x.ParentTaskId, op => op.MapFrom(x=>x.ParentWorkTaskId))
                .ForMember(x=>x.ParentTask, op=>op.MapFrom(x=>x.ParentWorkTask))
                .ForMember(x=>x.Description, op =>op.MapFrom(x=>x.Description))
                .ReverseMap();
        }
    }
}
