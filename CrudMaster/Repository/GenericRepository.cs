using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using AutoMapper;
using CrudMaster.Filter;
using CrudMaster.PropertyMapper;
using CrudMaster.Service;
using CrudMaster.Sorter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using X.PagedList;

namespace CrudMaster.Repository
{
    public abstract class GenericRepository<TEntity, TContext, TOrderByPredicateCreator, TFilterPredicateCreator> :

        IGenericRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
        where TOrderByPredicateCreator : IOrderByPredicateCreator<TEntity>, new()
        where TFilterPredicateCreator : IWherePredicateCreator<TEntity>, new()
    {
        public Expression<Func<TEntity, bool>> CustomWherePredicate { get; set; } = null;
        protected TContext Db { get; private set; }

        protected IMapper Mapper;
        protected GenericRepository(TContext context, IMapper mapper)
        {
            Db = context;
            Mapper = mapper;
        }

        public TEntity NewDbSet() => Db.CreateProxy<TEntity>();

        public Type GetTypeOfLinkedTableByForeignKeyName(Type typeOfEntity, string fkEntityName)
        {
            var allFks = Db.Model.FindEntityType(typeOfEntity).GetForeignKeys();

            var foundFk = allFks.Single(s => (s.Properties.Select(d => d.Name)).Contains(fkEntityName));
            if (foundFk == null)
                throw new Exception($"{fkEntityName} is not a foreign key of {typeOfEntity.ToString()}");

            var typeOfLinkedTable = foundFk.PrincipalEntityType;
            var clrType = typeOfLinkedTable.ClrType;
            return typeOfEntity.Assembly
                .GetType(clrType.FullName ??
                         throw new InvalidOperationException("ClrType does not have fullName of linked table Type"));
        }

        public Dictionary<string, string> OptionsForForeignKey(string fkDto, string template, string concatenator)
        {
            FindServiceThatHasCurrentRepository();
            var templateWithColumnNames = new TemplateWithColumnNames(template);

            var assemblyOfDb = Db.GetType().Assembly;
            var typesThatImplementsPropertyMapperInterfaceOfTEntity =
                assemblyOfDb.GetTypesThatImplementsGenericInterface(typeof(IPropertyMapper<>), new List<Type>() { typeof(TEntity) });

            IPropertyMapper<TEntity> proppertyMapper;
            if (typesThatImplementsPropertyMapperInterfaceOfTEntity.Count == 1)
                proppertyMapper = (IPropertyMapper<TEntity>)Activator.CreateInstance(typesThatImplementsPropertyMapperInterfaceOfTEntity.First());
            else
                throw new Exception("PropertyMapper not implemented for entity" + typeof(TEntity));


            var fkExpression = proppertyMapper.GetCorespondingPropertyNavigationInEntityForDtoField(fkDto);
            var fkAsString = fkExpression.GetExpressionBodyAsString();


            var allFks = Db.Model.FindEntityType(typeof(TEntity)).GetForeignKeys();

            var fkEntity = allFks.Single(s => s.Properties.Select(d => d.Name).Contains(fkAsString));
            var nameOfLinkedTable = fkEntity.PrincipalEntityType.Name;


            var linkedTableType = assemblyOfDb.GetType(nameOfLinkedTable);
            var dbSetOflinkedTable = (IQueryable<object>)Db.GetType().GetMethod("Set").MakeGenericMethod(linkedTableType).Invoke(Db, null);

            var pkOfLinkedTable = Db.Model.FindEntityType(linkedTableType).FindPrimaryKey().Properties.Select(y => y.Name).Single();

            dynamic linkedTableProppertyMapper;
            var z = typeof(IPropertyMapper<>).MakeGenericType(linkedTableType);

            var typesThatImplementsPropertyMapperInterfaceOfRelatedTEntity =
                assemblyOfDb.GetTypesThatImplementsGenericInterface(typeof(IPropertyMapper<>), new List<Type>() { linkedTableType });

            if (typesThatImplementsPropertyMapperInterfaceOfRelatedTEntity.Count == 1)
                linkedTableProppertyMapper = Activator.CreateInstance(typesThatImplementsPropertyMapperInterfaceOfRelatedTEntity.First());
            else
                throw new Exception("PropertyMapper not implemented for entity" + typeof(TEntity));


            //var i = 0;
            var colNames = templateWithColumnNames.GetDtoColumnNames();
            var matches = templateWithColumnNames.Matches;
            //var mappedColnamesAsStrings = new string[colNames.Length];

            var templateToMappedColNameDictionary = new Dictionary<string, string>();

            foreach (Match match in matches)
            {
                var dtoColName = match.Groups[1].Value;
                var colNameExpression = linkedTableProppertyMapper.GetCorespondingPropertyNavigationInEntityForDtoField(dtoColName);
                var colNameAsString = ExpressionExtensions.NonExtenionGetExpressionBodyAsString(colNameExpression);
                if (!templateToMappedColNameDictionary.ContainsKey(dtoColName))
                    templateToMappedColNameDictionary.Add(dtoColName, colNameAsString);
            }

            var res = dbSetOflinkedTable.Select(x =>
                new KeyValuePair<string, string>(
                    x.GetType().GetProperty(pkOfLinkedTable).GetValue(x).ToString(),
                    templateWithColumnNames.Replace(x, templateToMappedColNameDictionary)
                //ConcatColValues(x, mappedColnamesAsStrings, concatenator)
                )
            ).ToList();
            return res?.ToDictionary(x => x.Key, x => x.Value);
        }

        public Dictionary<string, string> OptionsForForeignKeyTest(Type linkedTableType, TemplateWithColumnNames template)
        {
            var includeStrs = new Dictionary<string, string>();
            foreach (var exp in template.ExpressionsOfDtoToEntityColNames.Values)
            {
                var expressionString = exp.ToString();
                var regex = new Regex(@"(\w+)\ \=\> \1\.{1}");
                var expPropNavigatior = regex.Match(expressionString).Value;
                var name = expressionString.Replace(expPropNavigatior, "");

                if (!name.Contains(".")) continue;

                // ReSharper disable once StringLastIndexOfIsCultureSpecific.1 
                var lastPosOfDot=name.LastIndexOf(".");
                name = name.Substring(0, lastPosOfDot);
                if (!includeStrs.ContainsKey((name)))
                    includeStrs.Add(name, name);
            }
            var pkOfLinkedTable = Db.Model.FindEntityType(linkedTableType).FindPrimaryKey().Properties.Select(y => y.Name).Single();
            var linkedTableType1 = Db.GetType().Assembly.GetType(linkedTableType.FullName);
            var dbSetOflinkedTable =
                (IQueryable<object>)Db.GetType().GetMethod("Set").MakeGenericMethod(linkedTableType1).Invoke(Db, null);

            //var xx = dbSetOflinkedTable;
            foreach (var includeStr in includeStrs)
            {
                dbSetOflinkedTable = dbSetOflinkedTable.Include(includeStr.Value);
            }

            var res=dbSetOflinkedTable.Select(x =>
                new KeyValuePair<string, string>(
                    x.GetType().GetProperty(pkOfLinkedTable).GetValue(x).ToString(),
                    template.Replace(x)
                )
            ).ToList();
            //var x = dbSetOflinkedTable.ToList().Select(s => Test1(s, exps[0])).ToList();
            return res?.ToDictionary(x => x.Key, x => x.Value);
        }
        public dynamic Test1(dynamic entity, LambdaExpression exps)
        {
            var x = entity.Region.Name.ToString();
            return x;
        }
        public dynamic Test(dynamic entity, List<LambdaExpression> exps)
        {
            string s = "";
            foreach (var exp in exps)
            {
                var compiledLambda = exp.Compile();
                var result = compiledLambda.DynamicInvoke(entity);
                s += result.ToString() + " ";
            }

            return s;
        }
        public void FindServiceThatHasCurrentRepository()
        {
            var assemblyOfDb = Db.GetType().Assembly;
            var thisType = this.GetType();
            var z = assemblyOfDb.GetTypes()
                .Where(t =>
                   t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(GenericService<,,,>))
                .ToList();
            dynamic s;
            foreach (var service in z)
            {
                var genericTypeArguments = service.BaseType.GenericTypeArguments.Where(ssss => ssss.IsInterface).ToList();
                foreach (var type in genericTypeArguments)
                {
                    var t = type.IsAssignableFrom(thisType);
                    if (t)
                    {
                        s = service;
                    }
                }
            }

            return;
        }


        private dynamic GetMapp(string dtoColName, Type type)
        {
            var x = Mapper.ConfigurationProvider.GetAllTypeMaps().Select(s => s.SourceType == type);

            //foreach (var typeMap in x)
            //{
            //    foreach (var typeMapPathMap in typeMap.PropertyMaps)
            //    {

            //    }
            //}

            return null;
        }

        public TEntity Find(int id) => Db.Set<TEntity>().Find(id);

        public virtual int DeleteAndReturn(int id)
        {
            Delete(id);
            return id;
        }

        public virtual void Delete(int id)
        {
            var entity = Find(id);
            ShouldDeleteEntity(entity);
            Db.Set<TEntity>().Remove(entity ?? throw new Exception($"Database is missing record with id:{id}"));
            Db.SaveChanges();
        }

        public virtual IPagedList<TEntity> Filter(Pager pager, string filters, OrderByProperties orderByProperties)
        {
            var orderBy = new TOrderByPredicateCreator().GetPropertyObject(orderByProperties);
            var filterPredicate = new TFilterPredicateCreator().GetWherePredicate(filters);

            IQueryable<TEntity> listOfEntities = Db.Set<TEntity>();

            var listOfFilteredEntities = filterPredicate == null ? listOfEntities : listOfEntities.Where(filterPredicate);
            listOfFilteredEntities = ApplyCustomCondition(listOfFilteredEntities);

            if (CustomWherePredicate != null)
                listOfFilteredEntities = listOfFilteredEntities.Where(CustomWherePredicate);

            var listOfOrderedEntities = orderByProperties.OrderDirection == SortDirections.Ascending
                ? listOfFilteredEntities.OrderBy(orderBy.OrderByProperty)
                : listOfFilteredEntities.OrderByDescending(orderBy.OrderByProperty);

            var pagedList = Paged(listOfOrderedEntities, pager);
            return pagedList;
        }



        protected virtual IQueryable<TEntity> ApplyCustomCondition(IQueryable<TEntity> listOfFilteredEntities) => listOfFilteredEntities;

        protected IPagedList<TEntity> Paged(IEnumerable<TEntity> listOfEntities, Pager pager)
        {
            if (pager.NumOfRowsPerPage < 0)
                return listOfEntities.ToPagedList();

            return listOfEntities.ToPagedList(pager.CurrentPageNumber, pager.NumOfRowsPerPage);
        }

        public virtual void Create(TEntity entity)
        {
            entity = ModifyEntityBeforeCreate(entity);
            Db.Set<TEntity>().Add(entity);
            Db.SaveChanges();
        }

        public virtual TEntity CreateAndReturn(TEntity entity)
        {
            Create(entity);
            return entity;
        }

        protected virtual TEntity ModifyEntityBeforeCreate(TEntity entity) => entity;

        public virtual void Update(TEntity entity)
        {
            var id = GetPrimaryKeyValue(entity);
            var oldEntity = Db.Set<TEntity>().Find(id);
            entity = ModifyUpdateSourceEntityBeforeUpdate(entity, oldEntity);
            Db.Entry(oldEntity).CurrentValues.SetValues(entity);
            Db.SaveChanges();
        }

        public virtual TEntity UpdateAndReturn(TEntity entity)
        {
            var id = GetPrimaryKeyValue(entity);
            var oldEntity = Db.Set<TEntity>().Find(id);
            entity = ModifyUpdateSourceEntityBeforeUpdate(entity, oldEntity);
            Db.Entry(oldEntity).CurrentValues.SetValues(entity);
            Db.SaveChanges();

            return oldEntity;

        }

        protected virtual TEntity ModifyUpdateSourceEntityBeforeUpdate(TEntity updateSourceEntity, TEntity oldEntity) => updateSourceEntity;

        protected virtual void ShouldDeleteEntity(TEntity entity) { }

        public virtual object GetPrimaryKeyValue(TEntity entity)
        {
            var pkName = Db.Entry(entity)
                .Metadata
                .FindPrimaryKey()
                .Properties
                .Select(x => x.Name)
                .ToList()
                .First()
                .ToString();


            return entity.GetType().GetProperty(pkName)?.GetValue(entity);
        }
    }
}