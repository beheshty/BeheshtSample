using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Core.Models.Paging
{
    public class PagedListOutputMeta
    {
        public string Search { get; set; } = "";
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItemCount { get; set; }
        public SearchType SearchType { get; set; }
        public bool IsFirstPage => !this.HasPreviousPage;
        public bool IsLastPage => !this.HasNextPage;
        public bool HasPreviousPage => this.PageNumber > 1;
        public bool HasNextPage => this.PageNumber < this.PageCount;
        public int PageCount => (int)Math.Ceiling(this.TotalItemCount / (double)(this.PageSize == 0 ? 10 : this.PageSize));
        public int NextPageNumber => this.HasNextPage ? this.PageNumber + 1 : this.PageCount;
        public int PreviousPageNumber => this.HasPreviousPage ? this.PageNumber - 1 : 1;
        public IEnumerable<ColumnFilter> ColumnFilters { get; set; } = new HashSet<ColumnFilter>();
        public ColumnSorting ColumnSorting { get; set; } = null;

    }
}
