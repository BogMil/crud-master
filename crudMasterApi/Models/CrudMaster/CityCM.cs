using CrudMaster;
using CrudMasterApi.Entities;

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

    public class CityCommandDto : CityBase
    {
    }

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

    public class CityMappingProfile : CrudMasterMappingProfile<CityQueryDto, CityCommandDto, City>
    {
        public override void PopulateMps()
        {
            //EntityToQueryDto.Add(x => x.Region, o => o.MapFrom(s => s.Region));
            //EntityToQueryDto.Add(x => x.RegionName, o => o.MapFrom(s => testera(s.Region.Name) + " " + testera(s.Region.Name)));
            EntityToQueryDto.Add(x => x.RegionName, o => o.MapFrom(s => s.Region.Name));
            //EntityToQueryDto.Add(x => x.RegionName, o => o.MapFrom(s => testera(s.Region.Name)));
        }

        public string testera(string s)
        {
            return s.ToString();
        }
    }

    public class Testera
    {
        public string Test(City s)
        {
            return s.Region.Name.ToLower();
        }
    }

    
}

