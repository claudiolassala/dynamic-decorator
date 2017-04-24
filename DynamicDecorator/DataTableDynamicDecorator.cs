using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DynamicDecorator
{
    public class DataTableDynamicDecorator : DynamicDtoDecorator
    {
        public DataTableDynamicDecorator(object dto) : base(dto)
        {
        }

        public static List<dynamic> MakeListFrom(DataTable dataTable)
        {
            return (from object row in dataTable.Rows select new DataTableDynamicDecorator(row)).Cast<dynamic>().ToList();
        }

        protected override void RegisterProperties(object dto)
        {
            var row = dto as DataRow;
            foreach (DataColumn column in row.Table.Columns)
            {
                Func<string, object> valueGetter = p => row[column.ColumnName];
                Action<string, object> valueSetter =
                    (p, newValue) => row[column.ColumnName] = newValue;
                Register(column.ColumnName, valueGetter, valueSetter);
            }
        }
    }
}