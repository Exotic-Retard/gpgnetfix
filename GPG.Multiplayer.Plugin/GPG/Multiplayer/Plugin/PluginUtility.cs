namespace GPG.Multiplayer.Plugin
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public static class PluginUtility
    {
        public static DataTable ConvertToDataTable(List<List<string>> data, List<string> columns)
        {
            DataTable table = new DataTable();
            int num = 0;
            foreach (string str in columns)
            {
                table.Columns.Add(str);
            }
            foreach (List<string> list in data)
            {
                DataRow row = table.NewRow();
                num = 0;
                foreach (string str2 in list)
                {
                    row[num] = str2;
                    num++;
                }
                table.Rows.Add(row);
            }
            return table;
        }
    }
}

