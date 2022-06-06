using System;
using System.Collections.Generic;
using System.Text;

namespace Behesht.Core.Models.Paging
{
    public class ColumnSorting
    {
        public string ColumnName { get; set; } = string.Empty;
        public SortType Type { get; set; } = SortType.Ascending;
    }
}
