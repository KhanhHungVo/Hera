using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.CryptoService
{
    abstract class BaseListService
    {
        protected virtual Dictionary<string,string> OrderByColumnMaps
        {
            get
            {
                return new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// Constructs the sort order clause based on the provided sort data. Sort terms must match items in
        /// the DTC's field dictionary.
        /// </summary>
        /// <param name="sortData">Sort data in the format "DTOField1,asc;DTOField2,desc;DTOField3"
        /// Default sort order is asc</param>
        /// <returns>string use in dynamic LinQ in format: "DTOField1 asc,DTOField2 desc,DTOField3 asc"</returns>
        protected string GetOrderByClause(string sortData)
        {
            try
            {
                StringBuilder orderByClause = new StringBuilder();
                bool hasPrimaryKeyValue = OrderByColumnMaps.ContainsKey("PrimaryKey");
                string firstOrderByColumnSortOrder = string.Empty;
                bool containPrimaryKeyValue = false;

                // If no sortby field is supplied, use the default.
                if (string.IsNullOrEmpty(sortData))
                {
                    return OrderByColumnMaps["default"] + " asc";
                }

                string[] sortItems = sortData.Split(';');

                foreach (var sortItem in sortItems)
                {
                    if (!string.IsNullOrEmpty(sortItem))
                    {
                        string[] splitedSortItems = sortItem.Split(',');
                        string orderByColumnName = splitedSortItems[0];

                        if (hasPrimaryKeyValue && String.Compare(OrderByColumnMaps[orderByColumnName], OrderByColumnMaps["PrimaryKey"], true) == 0)
                        {
                            containPrimaryKeyValue = true;
                        }

                        orderByClause.Append(OrderByColumnMaps[orderByColumnName]);

                        if (splitedSortItems.Length == 2)
                        {
                            string sortOrder = splitedSortItems[1].Equals("desc") ? " desc," : " asc,";
                            orderByClause.Append(sortOrder);
                            if (string.IsNullOrEmpty(firstOrderByColumnSortOrder))
                            {
                                firstOrderByColumnSortOrder = sortOrder;
                            }
                        }
                        else
                        {
                            orderByClause.Append(" asc,");  // Default sort order is asc
                        }
                    }
                }

                if (hasPrimaryKeyValue && !containPrimaryKeyValue)
                {
                    orderByClause.Append(OrderByColumnMaps["PrimaryKey"]);
                    if (!string.IsNullOrEmpty(firstOrderByColumnSortOrder))
                    {
                        orderByClause.Append(firstOrderByColumnSortOrder);
                    }
                    else
                    {
                        orderByClause.Append(" asc,");  // Default sort order is asc
                    }
                }

                return orderByClause.ToString().TrimEnd(',');
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
