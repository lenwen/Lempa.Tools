using System.Data;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace Lempa.Tools.Database
{
    public class DataTableToModel
    {
        public List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        private T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name.ToLower() == column.ColumnName)
                    {
                        // try
                        {
                            // if (dr[column.ColumnName].GetType() == System.Boolean)
                            if (dr[column.ColumnName] != null)
                            {
                                // if (typeof(T) == typeof(bool))
                                if (pro == typeof(bool))
                                {
                                    pro.SetValue(obj, Convert.ToBoolean(dr[column.ColumnName]), null);
                                }
                                else if (pro == typeof(string))
                                {
                                    //if (dr[column.ColumnName] is  DBNull)
                                    //    pro.SetValue(obj,"",null);
                                    //else if (dr[column.ColumnName].ToString() == "")
                                    //    pro.SetValue(obj, "", null);
                                    //else
                                    pro.SetValue(obj, dr[column.ColumnName].ToString(), null);
                                }
                                else if (pro == typeof(DBNull))
                                {
                                    pro.SetValue(obj, "", null);
                                }
                                else
                                {
                                    if (dr[column.ColumnName] is DBNull)
                                        pro.SetValue(obj, "", null);
                                    else
                                        // if (dr[column.ColumnName]. != typeof (DBNull))
                                        pro.SetValue(obj, dr[column.ColumnName], null);
                                }

                            }

                            // string dd = pro.GetType();



                        }
                        //catch (Exception)
                        //{

                        //    pro.SetValue(obj,"", null);
                        //}

                    }

                    else
                        continue;
                }
            }
            return obj;
        }
    }
}
