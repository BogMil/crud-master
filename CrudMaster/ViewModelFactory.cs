using System.Linq;
using X.PagedList;

namespace CrudMaster
{
    public static class ViewModelFactory
    {
        public static GenericViewModel<TQueryDto> CreateViewModel<TQueryDto> (StaticPagedList<TQueryDto> listOfDto) 
            where TQueryDto : class ,new () 
        {
            var viewModel = new GenericViewModel<TQueryDto>()
            {
                Records = listOfDto.ToList(),
                CurrentPageNumber = listOfDto.PageNumber,
                TotalNumberOfPages = listOfDto.PageCount,
                TotalNumberOfRecords = listOfDto.TotalItemCount,
            };

            return viewModel;

        } 
    }
}
