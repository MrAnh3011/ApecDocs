using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Common
{
    public static class DataTableEx
    {
        #region Mapping Data
        public static T ConvertToEntity<T>(this DataRow tableRow) where T : new()
        {
            var t = typeof(T);
            var returnObject = new T();
            foreach (DataColumn col in tableRow.Table.Columns)
            {
                var colName = col.ColumnName;
                var pInfos = t.GetProperties();
                foreach (var pInfo in pInfos)
                {
                    var val = tableRow[colName];
                    var attributies = pInfo.GetCustomAttributes(typeof(ColumnAttribute), false);
                    if (attributies.Any())
                    {
                        var attr = (ColumnAttribute)attributies[0];
                        if (attr.Name.Equals(colName.ToUpper()))
                        {
                            var isNullable = (Nullable.GetUnderlyingType(pInfo.PropertyType) != null);
                            if (isNullable)
                            {
                                val = Convert.ChangeType(val, Nullable.GetUnderlyingType(pInfo.PropertyType));
                            }
                            else
                            {
                                if (val is DBNull)
                                {
                                    val = null;
                                }
                                else
                                {
                                    if (pInfo.PropertyType.GetType() != typeof(string) &&
                                        string.IsNullOrEmpty(val.ToString()))
                                    {
                                        if (pInfo.PropertyType.GetType() == typeof(DateTime))
                                        {
                                            val = DateTime.MinValue;
                                        }
                                        else
                                        {
                                            val = "0";
                                        }
                                    }

                                    val = Convert.ChangeType(val, pInfo.PropertyType);
                                }
                            }
                            pInfo.SetValue(returnObject, val, null);
                        }
                    }
                }
            }
            return returnObject;
        }

        public static List<T> ConvertToList<T>(this DataTable table) where T : new()
        {
            return (from DataRow dr in table.Rows select dr.ConvertToEntity<T>()).ToList();
        }

        public static DataTable ConvertToDataTable(this object obj)
        {
            var pInfos = obj.GetType().GetProperties();
            var table = new DataTable();
            foreach (var pInfo in pInfos)
            {
                table.Columns.Add(pInfo.Name, pInfo.GetType());
            }

            var row = table.NewRow();
            foreach (var pInfo in pInfos)
            {
                row[pInfo.Name] = pInfo.GetValue(obj, null);
            }

            return table;
        }
        #endregion
    }
}
