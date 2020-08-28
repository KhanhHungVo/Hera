using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Hera.Common.Helper
{
    public static class SortHelper<T>
    {
        public static Expression<Func<T,object>> GetOrderExpression(string sortColumn)
        {
            return t => t.GetType().GetProperty(sortColumn).GetValue(t, null);
        }

        public static IEnumerable<T> SortBy(IEnumerable<T> source, string sortColumn, string sortOrder)
        {
            if (sortOrder == "asc")
            {
                return source.AsQueryable().OrderBy(GetOrderExpression(sortColumn));
            } else
            {
                return source.AsQueryable().OrderByDescending(GetOrderExpression(sortColumn));
            }
        }
    }
}
