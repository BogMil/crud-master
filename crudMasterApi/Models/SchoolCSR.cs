using System;
using System.Linq.Expressions;
using AutoMapper;
using CrudMaster;
using CrudMaster.Filter;
using CrudMaster.PropertyMapper;
using CrudMaster.Sorter;
using crudMasterApi.Entities;
using X.PagedList;

namespace crudMasterApi.Models
{
    public class SchoolBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public int CityId { get; set; }
    }

    public class SchoolQueryDto : SchoolBase
    {
        public CityQueryDto City { get; set; }
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
        public override Expression<Func<School, dynamic>> GetPathInEfForDtoFieldExpression(string fieldName)
        {
            if (fieldName == GetDtoPropertyPathAsString(t => t.Id))
                return x => x.Id;
            if (fieldName == GetDtoPropertyPathAsString(t => t.Mail))
                return x => x.Mail;
            if (fieldName == GetDtoPropertyPathAsString(t => t.Name))
                return x => x.Name;
            if (fieldName == GetDtoPropertyPathAsString(t => t.CityId))
                return x => x.City;

            throw new Exception("Putem requesta je poslato nepostojece polje " + fieldName +
            "  Obezbediti da za svako polje iz QueryDto modela postoji odgovarajuce mapiranje u entity modelu (bazi).");
        }

    }

    public class SchoolMappingProfile : Profile
    {
        public SchoolMappingProfile()
        {
            CreateMap<School, SchoolQueryDto>()
                .ForMember(d => d.City, o => o.MapFrom(s => s.City));

            CreateMap<SchoolCommandDto, School>()
                .ForMember(x => x.City, o => o.Ignore());

            CreateMap<PagedList<School>, StaticPagedList<SchoolQueryDto>>()
                .ConvertUsing<PagedListConverter<School, SchoolQueryDto>>();
        }
    }
}
