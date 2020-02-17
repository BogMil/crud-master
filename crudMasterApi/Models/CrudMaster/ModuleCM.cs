using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using CrudMaster;
using CrudMaster.Filter;
using CrudMaster.PropertyMapper;
using CrudMaster.Sorter;
using CrudMasterApi.Entities;
using CrudMasterApi.School;
using X.PagedList;

namespace CrudMasterApi.Models.CrudMaster
{
    public class ModuleBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Principal { get; set; }
        public int SchoolId { get; set; }
    }

    public class ModuleQueryDto : ModuleBase
    {
        public virtual SchoolQueryDto School { get; set; }

    }

    public class ModuleCommandDto : ModuleBase
    {
    }

    //public class ModuleMappingProfile : Profile
    //{
    //    public ModuleMappingProfile()
    //    {
    //        CreateMap<Module, ModuleQueryDto>()
    //            .ForMember(d => d.School, o => o.MapFrom(s => s.School));

    //        CreateMap<ModuleCommandDto, Module>()
    //            .ForMember(s => s.SubjectsOfModule, o => o.Ignore())
    //            .ForMember(s => s.School, o => o.Ignore());

    //        CreateMap<PagedList<Module>, StaticPagedList<ModuleQueryDto>>()
    //            .ConvertUsing<PagedListConverter<Module, ModuleQueryDto>>();
    //    }
    //}

    public class ModuleMappingProfile : CrudMasterMappingProfile<ModuleQueryDto, ModuleCommandDto, Module>
    {
        public override void PopulateMps()
        {
            EntityToQueryDto.Add(d => d.School, o => o.MapFrom(s => s.School));

            CommandDtoToEntity.Add(s=>s.School,o=>o.Ignore());
            CommandDtoToEntity.Add(s=>s.SubjectsOfModule, o => o.Ignore());
        }
    }
}