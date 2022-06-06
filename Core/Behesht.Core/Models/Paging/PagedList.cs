using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Core.Models.Paging
{
    public class PagedList<T>
    {
        public PagedList()
        {

        }

        public PagedList(PagedListInputMeta meta, int totalCount)
        {
            FillMeta(meta);
            MetaData.TotalItemCount = totalCount;
        }

        public PagedList(IList<T> data, PagedListInputMeta meta, int? totalCount = null)
        {
            FillMeta(meta);
            MetaData.TotalItemCount = totalCount ?? data.Count;
            MetaData.PageSize = Math.Max(MetaData.PageSize, 1);
            Data = !totalCount.HasValue ? data : data.Skip(MetaData.PageSize * (MetaData.PageNumber - 1)).Take(meta.PageSize).ToList();
        }

        private void FillMeta(PagedListInputMeta meta)
        {
            MetaData = new PagedListOutputMeta()
            {
                ColumnFilters = meta.ColumnFilters,
                ColumnSorting = meta.Sort,
                PageNumber = meta.PageNumber,
                PageSize = meta.PageSize,
                Search = meta.Search,
                SearchType = meta.SearchType
            };
        }

        public PagedListOutputMeta MetaData { get; set; }
        public IList<T> Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "";
        public int Status { get; set; } = 200;
    }
}
