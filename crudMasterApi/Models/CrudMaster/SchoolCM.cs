using System;
using System.Collections.Generic;
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
        public List<ModuleQueryDto> Modules { get; set; }
        public string RegionName { get; set; }

    }

    public class SchoolCommandDto : SchoolBase
    {
    }

    public class SchoolViewModel : GenericViewModel<SchoolQueryDto> { }

    public class SchoolOrderByPredicateCreator : GenericOrderByPredicateCreator<School, SchoolPropertyMapper>
    {
        protected override Expression<Func<School, dynamic>> GetDefaultOrderByColumn()
        {
            return x => x.Id;
        }
    }

    public class SchoolWherePredicateCreator : GenericWherePredicateCreator<School, SchoolPropertyMapper> { }

    public class SchoolPropertyMapper : GenericPropertyMapper<School, SchoolQueryDto>
    {
        public override Expression<Func<School, dynamic>> GetCorespondingPropertyNavigationInEntityForDtoField(string fieldName)
        {
            fieldName = fieldName.ToLower();
            if (fieldName == GetExpressionBodyWithoutParameterToLower(t => t.Id))
                return x => x.Id;
            if (fieldName == GetExpressionBodyWithoutParameterToLower(t => t.Mail))
                return x => x.Mail;
            if (fieldName == GetExpressionBodyWithoutParameterToLower(t => t.Name))
                return x => x.Name;
            if (fieldName == GetExpressionBodyWithoutParameterToLower(t => t.DtoCityId))
                return x => x.CityId;

            throw new Exception("Putem requesta je poslato nepostojece polje " + fieldName +
            "  Obezbediti da za svako polje iz QueryDto modela postoji odgovarajuce mapiranje u entity modelu (bazi).");
        }

    }

    public class SchoolMappingProfile : Profile
    {
        public SchoolMappingProfile()
        {
            CreateMap<School, SchoolQueryDto>()
                .ForMember(d => d.DtoCityId, o => o.MapFrom(s => s.CityId))
                .ForMember(d => d.RegionName, o => o.MapFrom(s => s.City.Region.Name));

            CreateMap<SchoolCommandDto, School>()
                .ForMember(x => x.City, o => o.Ignore())
                .ForMember(x => x.Modules, o => o.Ignore())
                .ForMember(d => d.CityId, o => o.MapFrom(s => s.DtoCityId));

            CreateMap<PagedList<School>, StaticPagedList<SchoolQueryDto>>()
                .ConvertUsing<PagedListConverter<School, SchoolQueryDto>>();
        }
    }
}
