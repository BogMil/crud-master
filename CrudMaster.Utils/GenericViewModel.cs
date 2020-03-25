using System.Collections.Generic;

namespace CrudMaster
{
    public class GenericViewModel<TQueryDto>
    {
        public int CurrentPageNumber { get; set; }
        public int TotalNumberOfPages { get; set; }
        public int TotalNumberOfRecords { get; set; }
        public List<TQueryDto> Records { get; set; }
    }
}
