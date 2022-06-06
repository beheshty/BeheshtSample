using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Core.Models.Paging
{
    public class PagedListInputMeta
    {
        public string Search { get; set; } = "";
        public SearchType SearchType { get; set; } = SearchType.Like;
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public List<ColumnFilter> ColumnFilters { get; set; } = new List<ColumnFilter>();
        public ColumnSorting Sort { get; set; }

    }
}
