using CrudMaster.Filter;
using CrudMaster.Sorter;
using CrudMaster.Utils;
using X.PagedList;

namespace CrudMaster
{
    public interface IGenericService<TQueryDto, TCommandDto>
    {
        void Create(TCommandDto dto);
        void Update(TCommandDto dto);
        void Delete(int id);
        TQueryDto Find(int id);
        StaticPagedList<TQueryDto> GetJqGridDataTest(Pager pager, string filters, OrderByProperties orderByProperties);
        //Dictionary<string, string> OptionsForForeignKey(string fkName, string templateWithColumnNames, string concatenator);
    }
    public abstract class GenericService<TQueryDto, TCommandDto, TRepository, TEntity>
        : IGenericService<TQueryDto, TCommandDto>
        where TQueryDto : class
        where TCommandDto : class
        where TRepository : IGenericRepository<TEntity>
        where TEntity : class
    {
        protected TRepository Repository { get; }
        protected IMappingService MappingService { get; }
        protected GenericService(TRepository repository)
        {
            Repository = repository;
            MappingService = new MappingService();
        }

        public virtual StaticPagedList<TQueryDto> GetJqGridDataTest(Pager pager, string filters, OrderByProperties orderByProperties)
        {
            var wherePredicate = FilterFactory.Create<TEntity, TQueryDto>(filters);
            var orderBy = new GenericOrderByPredicateCreator<TEntity, TQueryDto>().GetPropertyObject(orderByProperties);

            var includings = MappingService.GetIncludings(typeof(TEntity), typeof(TQueryDto));
            
            var entities = Repository.Filter(pager, wherePredicate, orderBy, includings);

            //var res = Repository
            //    .RecordSelector()
            //    .Include(includings).;
            //var records = Repository
            //    .RecordSelector()
            //    .Include()
            //    .Where(wherePredicate)
            //    .
            //    .
            //    .Where()
            //    .OrderBy
            //    .ThenBy
            //    .thenBy
            //    .Paginate();

            return MappingService.MapToStaticPageList<TEntity,TQueryDto>(entities);
        }

        public virtual void Create(TCommandDto dto)
        {
            ValidateDtoBeforeCreate(dto);
            ValidateDtoBeforeUpdateOrCreate(dto);

            var entity = Repository.GetNewDbSet();

            MappingService.Mapper.Map(dto, entity);

            Repository.Create(entity);
        }

        public virtual void ValidateDtoBeforeCreate(TCommandDto dto)
        {

        }

        public virtual void Update(TCommandDto dto)
        {
            ValidateDtoBeforeUpdate(dto);
            ValidateDtoBeforeUpdateOrCreate(dto);

            var entity = Repository.GetNewDbSet();

            MappingService.Mapper.Map(dto, entity);

            Repository.Update(entity);
        }

        public virtual void ValidateDtoBeforeUpdate(TCommandDto dto)
        {

        }

        public virtual void ValidateDtoBeforeUpdateOrCreate(TCommandDto dto)
        {

        }
        public virtual void Delete(int id)
        {
            Repository.Delete(id);
        }
        public virtual void ValidateDtoBeforeDelete(TCommandDto dto)
        {

        }

        public TQueryDto Find(int id)
        {
            var entity = Repository.Find(id);
            return MappingService.Mapper.Map<TEntity, TQueryDto>(entity);
        }
    }
}