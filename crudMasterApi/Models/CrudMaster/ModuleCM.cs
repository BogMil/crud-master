using CrudMasterApi.School;

namespace CrudMasterApi.Models.CrudMaster
{
    public class ModuleBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Principal { get; set; }
        public int SchoolId { get; set; }
    }

    public class ModuleQueryDto : ModuleBase
    {
        public virtual SchoolQueryDto School { get; set; }

    }

    public class ModuleCommandDto : ModuleBase
    {
    }

    //public class ModuleMappingProfile : Profile
    //{
    //    public ModuleMappingProfile()
    //    {
    //        CreateMap<Module, ModuleQueryDto>()
    //            .ForMember(d => d.School, o => o.MapFrom(s => s.School));

    //        CreateMap<ModuleCommandDto, Module>()
    //            .ForMember(s => s.SubjectsOfModule, o => o.Ignore())
    //            .ForMember(s => s.School, o => o.Ignore());

    //        CreateMap<PagedList<Module>, StaticPagedList<ModuleQueryDto>>()
    //            .ConvertUsing<PagedListConverter<Module, ModuleQueryDto>>();
    //    }
    //}

    //public class ModuleMappingProfile : CrudMasterMappingProfile<ModuleQueryDto, ModuleCommandDto, Module>
    //{
    //    public override void PopulateMps(Dictionary<Expression<Func<ModuleQueryDto, dynamic>>, Expression<Action<IMemberConfigurationExpression<Module, ModuleQueryDto, dynamic>>>> entityToQueryDto)
    //    {
    //        entityToQueryDto.Add(d => d.School, o => o.MapFrom(s => s.School));

    //        CommandDtoToEntity.Add(s => s.School, o => o.Ignore());
    //        CommandDtoToEntity.Add(s => s.SubjectsOfModule, o => o.Ignore());
    //    }

    //    public override void ConfigureEntityToQueryDtoMap(IMapFrom<Module, ModuleQueryDto> map)
    //    {
            
    //    }
    //}
}