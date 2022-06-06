using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Behesht.Core.Models.Paging;

namespace System.Linq
{
    public static class IQueryableExtension
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> query, PagedListInputMeta meta, bool sortOutput = true)
        {
            bool columnFilterEnabled = meta.ColumnFilters != null && meta.ColumnFilters.Any();
            bool isSorted = false;
            if (!string.IsNullOrWhiteSpace(meta.Search) || columnFilterEnabled)
            {
                string dynamicQuery = string.Empty;
                var properties = typeof(T).GetProperties();
                foreach (var item in properties)
                {
                    if (!item.PropertyType.IsSearchable() || item.Name.EndsWith("Id") || item.Name == "Id")
                    {
                        continue;
                    }
                    ColumnFilter cFilter = null;
                    if (columnFilterEnabled)
                    {
                        cFilter = meta.ColumnFilters.FirstOrDefault(p => p.ColumnName.Trim().ToLower() == item.Name.ToLower());
                        if (cFilter == null)
                        {
                            continue;
                        }
                    }

                    if (dynamicQuery != string.Empty)
                    {
                        if (columnFilterEnabled)
                        {
                            dynamicQuery += " && ";
                        }
                        else
                        {
                            dynamicQuery += " || ";
                        }
                    }

                    //Making dynamic query 
                    bool isString = item.PropertyType == typeof(string);
                    string search = columnFilterEnabled ? cFilter.Search : meta.Search;
                    if ((cFilter == null && meta.SearchType == SearchType.Like) || (cFilter != null && cFilter.SearchType == SearchType.Like))
                    {
                        if (isString)
                        {
                            dynamicQuery += $"{item.Name}.Contains(\"{search}\")";
                        }
                        else
                        {
                            dynamicQuery += $"{item.Name}.ToString().Contains(\"{search}\")";
                        }
                    }
                    else
                    {
                        if (isString)
                        {
                            dynamicQuery += $"({item.Name} == \"{search}\")";
                        }
                        else
                        {
                            dynamicQuery += $"{item.Name}.ToString() == \"{search}\"";
                        }
                    }

                    if (sortOutput && meta.Sort != null && !string.IsNullOrEmpty(meta.Sort.ColumnName) && item.Name.ToLower() == meta.Sort.ColumnName.ToLower())
                    {
                        query = query.OrderBy($"{item.Name}{(meta.Sort.Type == SortType.Ascending ? "" : " desc")}")
                            .ThenBy("Id desc");
                        isSorted = true;
                    }
                }
                if (dynamicQuery != string.Empty)
                {
                    query = query.Where(dynamicQuery);
                }

            }

            var totalCount = query.Count();

            if (sortOutput && !isSorted)
            {
                query = query.OrderBy("Id desc");
            }

            query = query.Skip(meta.PageSize * (meta.PageNumber - 1)).Take(meta.PageSize);

            PagedList<T> list = new PagedList<T>(meta, totalCount);
            list.Data = query.ToList();

            return list;
        }

        private static string SearchDate(Reflection.PropertyInfo item, string search)
        {
            string[] strDates = search.Split(',');
            string condition = "";
            if (strDates.Length >= 1)
            {
                if (DateTime.TryParse(strDates[0], out DateTime dt1))
                {
                    condition += $"({item.Name} != null ? {item.Name} >= DateTime({dt1.Year},{dt1.Month},{dt1.Day}) : false)";
                }
            }
            if (strDates.Length >= 2)
            {
                if (!string.IsNullOrEmpty(condition))
                {
                    condition += " && ";
                }
                if (DateTime.TryParse(strDates[1], out DateTime dt2))
                {
                    condition += $"({item.Name} != null ? {item.Name} <= DateTime({dt2.Year},{dt2.Month},{dt2.Day}) : false)";
                }
            }
            return string.IsNullOrEmpty(condition) ? " true " : condition;
        }
    }
}
