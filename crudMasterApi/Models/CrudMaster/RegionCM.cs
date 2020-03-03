namespace CrudMasterApi.Models.CrudMaster
{
    public class RegionBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NekiInt { get; set; }

    }

    public class RegionQueryDto : RegionBase
    {
    }

    public class RegionCommandDto : RegionBase
    {
    }

    //public class RegionMappingProfile : Profile
    //{
    //    public RegionMappingProfile()
    //    {
    //        CreateMap<Region, RegionQueryDto>()
    //            .ForMember(x => x.NekiInt, o => o.MapFrom(s => s.TestInt));

    //        CreateMap<RegionCommandDto, Region>()
    //            .ForMember(s => s.Cities, o => o.Ignore())
    //            .ForMember(s => s.TestInt, o => o.MapFrom(s=>s.NekiInt));

    //        CreateMap<PagedList<Region>, StaticPagedList<RegionQueryDto>>()
    //            .ConvertUsing<PagedListConverter<Region, RegionQueryDto>>();
    //    }
    //}

    //public class RegionMappingProfile : CrudMasterMappingProfile<RegionQueryDto, RegionCommandDto, Region>
    //{
    //    public override void PopulateMps(Dictionary<Expression<Func<RegionQueryDto, dynamic>>, Expression<Action<IMemberConfigurationExpression<Region, RegionQueryDto, dynamic>>>> entityToQueryDto)
    //    {
    //        entityToQueryDto.Add(x => x.NekiInt, o => o.MapFrom(s => s.TestInt));

    //        CommandDtoToEntity.Add(s => s.Cities, o => o.Ignore());
    //        CommandDtoToEntity.Add(s => s.TestInt, o => o.MapFrom(d => d.NekiInt));
    //    }

    //    public override void ConfigureEntityToQueryDtoMap(IMapFrom<Region, RegionQueryDto> map)
    //    {
    //    }
    //}
}