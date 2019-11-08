using System;
using System.Linq.Expressions;
using AutoMapper;
using CrudMaster;
using CrudMaster.Filter;
using CrudMaster.PropertyMapper;
using CrudMaster.Sorter;
using CrudMasterApi.Entities;
using X.PagedList;

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

    public class RegionMappingProfile : CrudMasterMappingProfile<RegionQueryDto, RegionCommandDto, Region>
    {
        public override void PopulateMps()
        {
            EntityToQueryDto.Add(x => x.NekiInt, o => o.MapFrom(s => s.TestInt));
        }
    }
}