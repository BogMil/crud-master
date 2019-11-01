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
    public class CityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PostalCode { get; set; }
    }

    public class CityQueryDto : CityBase
    {
    }

    public class CityCommandDto : CityBase
    {
    }

    public class CityViewModel : GenericViewModel<CityQueryDto> { }

    public class CityOrderByPredicateCreator : GenericOrderByPredicateCreator<City, CityPropertyMapper>
    {
        protected override Expression<Func<City, dynamic>> GetDefaultOrderByColumn()
        {
            return x => x.Id;
        }
    }

    public class CityWherePredicateCreator : GenericWherePredicateCreator<City, CityPropertyMapper> { }

    public class CityPropertyMapper : GenericPropertyMapper<City, CityQueryDto>
    {
        public override Expression<Func<City, dynamic>> GetCorespondingPropertyNavigationInEntityForDtoField(string fieldName)
        {
            if (fieldName == GetExpressionBodyWithoutParameter(t => t.Id)) 
                return x => x.Id;
            if (fieldName == GetExpressionBodyWithoutParameter(t => t.Name))
                return x => x.Name;
            if (fieldName == GetExpressionBodyWithoutParameter(t => t.PostalCode))
                return x => x.PostalCode;

            throw new Exception("Putem requesta je poslato nepostojece polje " + fieldName +
            "  Obezbediti da za svako polje iz QueryDto modela postoji odgovarajuce mapiranje u entity modelu (bazi).");
        }

    }

    public class CityMappingProfile : Profile
    {
        public CityMappingProfile()
        {
            CreateMap<City, CityQueryDto>();

            CreateMap<CityCommandDto, City>()
                .ForMember(s => s.Schools, o => o.Ignore())
                .ForMember(s => s.Schools, o => o.Ignore());

            CreateMap<PagedList<City>, StaticPagedList<CityQueryDto>>()
                .ConvertUsing<PagedListConverter<City, CityQueryDto>>();
        }
    }
}