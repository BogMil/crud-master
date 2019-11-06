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

    public class ModuleViewModel : GenericViewModel<ModuleQueryDto> { }

    public class ModuleOrderByPredicateCreator : GenericOrderByPredicateCreator<Module, ModulePropertyMapper>
    {
        protected override Expression<Func<Module, dynamic>> GetDefaultOrderByColumn()
        {
            return x => x.Id;
        }
    }

    public class ModuleWherePredicateCreator : GenericWherePredicateCreator<Module, ModulePropertyMapper> { }

    public class ModulePropertyMapper : GenericPropertyMapper<Module, ModuleQueryDto>
    {
        public override Expression<Func<Module, dynamic>> GetCorespondingPropertyNavigationInEntityForDtoField(string fieldName)
        {
            if (fieldName == GetExpressionBodyWithoutParameter(t => t.Id))
                return x => x.Id;
            if (fieldName == GetExpressionBodyWithoutParameter(t => t.Name))
                return x => x.Name;
            if (fieldName == GetExpressionBodyWithoutParameter(t => t.Principal))
                return x => x.Principal;
            if (fieldName == GetExpressionBodyWithoutParameter(t => t.SchoolId))
                return x => x.SchoolId;

            throw new Exception("Putem requesta je poslato nepostojece polje " + fieldName +
            "  Obezbediti da za svako polje iz QueryDto modela postoji odgovarajuce mapiranje u entity modelu (bazi).");
        }

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
        }
    }
}