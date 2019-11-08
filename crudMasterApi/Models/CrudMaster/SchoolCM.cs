using System.Collections.Generic;
using CrudMaster;
using CrudMasterApi.Entities;

namespace CrudMasterApi.Models.CrudMaster
{
    public class SchoolBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public int DtoCityId { get; set; }

    }

    public class SchoolQueryDto : SchoolBase
    {
        public CityQueryDto City { get; set; }
        //public List<ModuleQueryDto> Modules { get; set; }
        public string RegionName { get; set; }

    }

    public class SchoolCommandDto : SchoolBase
    {
    }


    //public class SchoolMappingProfile : Profile
    //{
    //    public SchoolMappingProfile()
    //    {
    //        CreateMap<School, SchoolQueryDto>()
    //            .ForMember(d => d.DtoCityId, o => o.MapFrom(s => s.CityId))
    //            .ForMember(d => d.RegionName, o => o.MapFrom(s => s.City.Region.Name));

    //        CreateMap<SchoolCommandDto, School>()
    //            .ForMember(x => x.City, o => o.Ignore())
    //            .ForMember(x => x.Modules, o => o.Ignore())
    //            .ForMember(d => d.CityId, o => o.MapFrom(s => s.DtoCityId));

    //        CreateMap<PagedList<School>, StaticPagedList<SchoolQueryDto>>()
    //            .ConvertUsing<PagedListConverter<School, SchoolQueryDto>>();
    //    }
    //}

    public class SchoolMappingProfile : CrudMasterMappingProfile<SchoolQueryDto, SchoolCommandDto, School>
    {
        public override void PopulateMps()
        {
            EntityToQueryDto.Add(d => d.DtoCityId, o => o.MapFrom(s => s.CityId));
            EntityToQueryDto.Add(d => d.RegionName, o => o.MapFrom(s => s.City.Region.Name));
        }
    }
}
