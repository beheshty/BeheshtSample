using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Core.Models.Paging
{
    public class ColumnFilter
    {
        public string ColumnName { get; set; } = "";
        public string Search { get; set; } = "";
        public SearchType SearchType { get; set; } = SearchType.Like;
    }
}
