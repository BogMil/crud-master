using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
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
        StaticPagedList<TQueryDto> Get(Pager pager, string filters, string orderProperties);
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

        public virtual StaticPagedList<TQueryDto> Get(Pager pager, string filters, string orderProperties)
        {
            var wherePredicate = WherePredicateFactory.Create<TEntity, TQueryDto>(filters);
            var orderInstructions = OrderInstructionsFactory.Create<TEntity, TQueryDto>(orderProperties);
            var includings = MappingService.GetIncludings(typeof(TEntity), typeof(TQueryDto));

            var entities = Repository
                .RecordSelector()
                .Include(includings)
                .Where(new List<Expression<Func<TEntity, bool>>> {wherePredicate})
                .ApplyOrders(orderInstructions)
                .Paginate(pager);
            
            return MappingService.MapToStaticPageList<TEntity,TQueryDto>(entities);
        }

        public virtual void Create(TCommandDto dto)
        {
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

    public static class OrderInstructionsFactory
    {
        public static List<OrderInstruction> Create<TEntity, TQueryDto>(string orderProperties)
        {
            if(!AreOrderPropertiesValid(orderProperties))
                return new List<OrderInstruction>();

            var listOfOrderProperties= JsonSerializer.Deserialize<List<OrderProperties>>(orderProperties);
            return new OrderPropertyMapper()
                .MapToListOfOrderInstructions<TEntity, TQueryDto>(listOfOrderProperties);
        }

        private static bool AreOrderPropertiesValid(string orderProperties)
            =>!(string.IsNullOrEmpty(orderProperties) || string.IsNullOrWhiteSpace(orderProperties));
        
    }

    public class OrderPropertyMapper
    {
        private readonly MappingService _ms;

        public OrderPropertyMapper()
        {
            _ms = new MappingService();
        }

        public List<OrderInstruction> MapToListOfOrderInstructions<TEntity, TQueryDto>(List<OrderProperties> listOfOrderProperties)
            =>listOfOrderProperties.Select(MapToOrderInstruction<TEntity, TQueryDto>).ToList();

        private OrderInstruction MapToOrderInstruction<TEntity, TQueryDto>(OrderProperties orderProperties)
        {
            var lambdaExpression =
                _ms.GetPropertyMapExpression(orderProperties.Column, typeof(TQueryDto), typeof(TEntity));
            return new OrderInstruction(
                lambdaExpression,
                orderProperties.Direction);
        }
    }
}