namespace CrudMasterApi.Models.CrudMaster
{
    public class CityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PostalCode { get; set; }
        public int RegionId { get; set; }
    }

    public class CityQueryDto : CityBase
    {
        public RegionQueryDto Region { get; set; }
        public string RegionName { get; set; }
    }

    public class CityCommandDto : CityBase { }

    //public class CityMappingProfile : Profile
    //{
    //    public CityMappingProfile()
    //    {
    //        CreateMap<City, CityQueryDto>()
    //            .ForMember(x => x.Region, o => o.MapFrom(s => s.Region))
    //            .ForMember(x => x.RegionName,
    //                o => o.MapFrom(s => s.Region.Name.ToString() + " " + s.Region.Name.ToString()));

    //        CreateMap<CityCommandDto, City>()
    //            .ForMember(s => s.Region, o => o.Ignore())
    //            .ForMember(s => s.Schools, o => o.Ignore());

    //        CreateMap<PagedList<City>, StaticPagedList<CityQueryDto>>()
    //            .ConvertUsing<PagedListConverter<City, CityQueryDto>>();
    //    }
    //}

    //public class CityMappingProfile : CrudMasterMappingProfile<CityQueryDto, CityCommandDto, City>
    //{
    //    public override void PopulateMps(Dictionary<Expression<Func<CityQueryDto, dynamic>>, Expression<Action<IMemberConfigurationExpression<City, CityQueryDto, dynamic>>>> entityToQueryDto)
    //    {
    //        entityToQueryDto.Add(x => x.RegionName, o => o.MapFrom(s => s.Region.Name));

    //        CommandDtoToEntity.Add(s => s.Region, o => o.Ignore());
    //        CommandDtoToEntity.Add(s => s.Schools, o => o.Ignore());
    //    }

    //    public override void ConfigureEntityToQueryDtoMap(IMapFrom<City, CityQueryDto> map)
    //    {
            
    //    }
    //}
}

