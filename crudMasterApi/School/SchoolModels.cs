using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using CrudMaster;
using CrudMasterApi.Models.CrudMaster;

namespace CrudMasterApi.School
{
    public class SchoolBase
    {
        public int? Id { get; set; } = 0;
        public string Name { get; set; }
        public string Mail { get; set; }
        public int DtoCityId { get; set; }
        public int NekiInt { get; set; }
        public int? NekiNullableInt { get; set; }
        public long NekiLong { get; set; }
        public bool NekiBool { get; set; }
        public decimal NekiDecimal { get; set; }
        public double NekiDouble { get; set; }
        public float NekiFloat { get; set; }
        public DateTime NekiDatum { get; set; }
    }

    public class SchoolQueryDto : SchoolBase
    {
        public CityQueryDto City { get; set; }
        public string CityName { get; set; }
        public string RegionName { get; set; }

    }

    public class SchoolCommandDto : SchoolBase { }
    public class SchoolMappingProfile : CrudMasterMappingProfile<SchoolQueryDto, SchoolCommandDto, Entities.School>
    {
        public override void PopulateMps(
            Dictionary<Expression<Func<SchoolQueryDto, dynamic>>, Expression<
                Action<IMemberConfigurationExpression<Entities.School, SchoolQueryDto, dynamic>>>> entityToQueryDto)
        {
            entityToQueryDto.Add(d => d.DtoCityId, o => o.MapFrom(s => s.CityId));
            entityToQueryDto.Add(d => d.CityName, o => o.MapFrom(s => s.City.Name+"ski"));

            entityToQueryDto.Add(d => d.RegionName, o => o.MapFrom(s => s.City.Region.Name));

            CommandDtoToEntity.Add(d => d.CityId, o => o.MapFrom(s => s.DtoCityId));
            //CommandDtoToEntity.Add(d => d.City, o => o.Ignore());
            //CommandDtoToEntity.Add(d => d.Modules, o => o.Ignore());
        }
    }
}
