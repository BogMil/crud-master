using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using CrudMaster;
using CrudMasterApi.Entities;

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
        public string test { get; set; }
    }

    public class RegionCommandDto : RegionBase
    {
    }

    public class RegionMappingProfile : CrudMasterMappingProfile<RegionQueryDto, RegionCommandDto, Region>
    {
        public override void MapToQueryDtoFromEntity(IMapTo<Region, RegionQueryDto> map)
        {
            map.To(d => d.NekiInt).From(s => s.TestInt)
                .To(d => d.test).From(s => s.Name+"-"+ s.Name);
        }

        public override void MapToEntityFromCommandDto(IMapTo<RegionCommandDto, Region> map)
        {
            map.To(d => d.TestInt).From(s => s.NekiInt);
        }
    }
}