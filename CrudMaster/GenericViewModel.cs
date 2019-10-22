using System.Collections.Generic;

namespace CrudMaster
{
    public class GenericViewModel<TQueryDto> where TQueryDto:class
    {
        public List<TQueryDto> Records;
        public int CurrentPageNumber;
        public int TotalNumberOfPages;
        public int TotalNumberOfRecords;
    }
}