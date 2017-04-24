using System.Collections.Generic;
using System.Data;
using FluentAssertions;
using NSpec;

namespace DynamicDecorator.Tests
{
    internal class data_table_spec : nspec
    {
        private DataTable _dataTable;
        private List<dynamic> _list;

        private void creating_list_off_data_table()
        {
            before = () =>
            {
                create_data_table();
                _list = DataTableDynamicDecorator.MakeListFrom(_dataTable);
            };

            it["creates list of proxies for rows in the data table"] = () =>
            {
                _list.Count.Should().Be(_list.Count);
                for (var i = 0; i < _dataTable.Rows.Count; i++)
                {
                    (_list[i].ColumnA as string).Should().Be("Value " + i);
                    ((int) _list[i].ColumnB).Should().Be(i);
                }
            };
        }

        private void create_data_table()
        {
            _dataTable = new DataTable();
            _dataTable.Columns.Add(new DataColumn("ColumnA", typeof(string)));
            _dataTable.Columns.Add(new DataColumn("ColumnB", typeof(int)));

            for (var i = 0; i < 10; i++)
            {
                var row = _dataTable.NewRow();
                row["ColumnA"] = "Value " + i;
                row["ColumnB"] = i;
                _dataTable.Rows.Add(row);
            }
        }
    }
}