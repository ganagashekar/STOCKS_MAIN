using STM_API.Model;
using System.Data;
using System.Reflection;

namespace STM_API.Extentions
{
    public static class ObjectExtention
    {
        public static  List<EquitiesHsitry> CustomSort<T>( List<EquitiesHsitry> input, string property,string Customorderby)
        {
            if (Customorderby == "asc")
            {
                var type = typeof(T);
                var sortProperty = type.GetProperty(property);
                return input.OrderBy(p => sortProperty.GetValue(p, null)).ToList();

            }
            else
            {
                var type = typeof(T);
                var sortProperty = type.GetProperty(property);
                return input.OrderByDescending(p => sortProperty.GetValue(p, null)).ToList();
            }
        }
        public static List<EquitiesHsitry> CustomSort<T>(this List<EquitiesHsitry> input, string property)
        {
            var type = typeof(T);
            var sortProperty = type.GetProperty(property);
            return input.OrderBy(p => sortProperty.GetValue(p, null)).ToList();
        }

        public static List<double> AddValue(this List<double> equities, double value)
        {
            equities.Add(value);
            return equities.ToList();
        }
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}
