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
        //public CityQueryDto City { get; set; }
        public string CityName { get; set; }
        public string RegionName { get; set; }

    }

    public class SchoolCommandDto : SchoolBase { }
    public class SchoolMappingProfile : CrudMasterMappingProfile<SchoolQueryDto, SchoolCommandDto, Entities.School>
    {
        public override void MapToQueryDtoFromEntity(IMapTo<Entities.School, SchoolQueryDto> map)
        {
            map.To(s => s.DtoCityId).From(s => s.CityId)
                .To(s => s.CityName).From(s => s.City.Name + "ski")
                .To(s => s.RegionName).From(s => s.City.Region.Name);
        }

        public override void MapToEntityFromCommandDto(IMapTo<SchoolCommandDto,Entities.School> map)
        {
            map.To(s => s.CityId).From(s => s.DtoCityId);
        }
    }

   
}
