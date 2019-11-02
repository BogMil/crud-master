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

    public class RegionViewModel : GenericViewModel<RegionQueryDto> { }

    public class RegionOrderByPredicateCreator : GenericOrderByPredicateCreator<Region, RegionPropertyMapper>
    {
        protected override Expression<Func<Region, dynamic>> GetDefaultOrderByColumn()
        {
            return x => x.Id;
        }
    }

    public class RegionWherePredicateCreator : GenericWherePredicateCreator<Region, RegionPropertyMapper> { }

    public class RegionPropertyMapper : GenericPropertyMapper<Region, RegionQueryDto>
    {
        public override Expression<Func<Region, dynamic>> GetCorespondingPropertyNavigationInEntityForDtoField(string fieldName)
        {
            fieldName = fieldName.ToLower();
            if (fieldName == GetExpressionBodyWithoutParameterToLower(t => t.Id))
                return x => x.Id;
            if (fieldName == GetExpressionBodyWithoutParameterToLower(t => t.Name))
                return x => x.Name;
            if (fieldName == GetExpressionBodyWithoutParameterToLower(t => t.NekiInt))
                return x => x.TestInt;

            throw new Exception("Putem requesta je poslato nepostojece polje " + fieldName +
            "  Obezbediti da za svako polje iz QueryDto modela postoji odgovarajuce mapiranje u entity modelu (bazi).");
        }

    }

    public class RegionMappingProfile : Profile
    {
        public RegionMappingProfile()
        {
            CreateMap<Region, RegionQueryDto>()
                .ForMember(x => x.NekiInt, o => o.MapFrom(s => s.TestInt));

            CreateMap<RegionCommandDto, Region>()
                .ForMember(s => s.Cities, o => o.Ignore())
                .ForMember(s => s.TestInt, o => o.MapFrom(s=>s.NekiInt));

            CreateMap<PagedList<Region>, StaticPagedList<RegionQueryDto>>()
                .ConvertUsing<PagedListConverter<Region, RegionQueryDto>>();
        }
    }
}