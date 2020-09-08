using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Hera.Common.Helper
{
    public static class SortHelper<T>
    {
        public static Expression<Func<T,object>> GetOrderExpression(string sortColumn)
        {
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(sortColumn, StringComparison.InvariantCultureIgnoreCase));
            if (objectProperty == null)
                return null;
            return t => t.GetType().GetProperty(sortColumn).GetValue(t, null);
        }

        public static IEnumerable<T> SortBy(IEnumerable<T> source, string sortColumn = "", string sortOrder = "")
        {
            var orderExp = GetOrderExpression(sortColumn);
            return sortOrder == "asc" ? source.AsQueryable().OrderBy(orderExp) : source.AsQueryable().OrderByDescending(orderExp);
        }
    }
}
