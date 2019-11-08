using System;
using System.Collections.Generic;
using System.Text;

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
