namespace GPG.DataAccess
{
    using System;
    using System.Collections.Generic;

    public class DataList : List<DataRecord>
    {
        public const char DefaultFieldSeparator = '|';
        public const char DefaultLineSeparator = '\x0003';

        public DataList()
        {
        }

        public DataList(string data)
        {
            this.ParseData(data, '\x0003', '|');
        }

        public DataList(List<string> columns, List<List<string>> rows)
        {
            string data = "";
            foreach (string str2 in columns)
            {
                data = data + str2 + '|';
            }
            data.TrimEnd(new char[] { '|' });
            data = data + '\x0003';
            foreach (List<string> list in rows)
            {
                foreach (string str3 in list)
                {
                    data = data + str3 + '|';
                }
                data.TrimEnd(new char[] { '|' });
                data = data + '\x0003';
            }
            data.TrimEnd(new char[] { '\x0003' });
            this.ParseData(data, '\x0003', '|');
        }

        public DataList(string data, char lineSeparator, char fieldSeparator)
        {
            this.ParseData(data, lineSeparator, fieldSeparator);
        }

        public string Describe()
        {
            string str = "";
            if (base.Count > 0)
            {
                foreach (DataRecord record in this)
                {
                    string[] array = new string[record.Count];
                    record.InnerHash.Keys.CopyTo(array, 0);
                    for (int i = 0; i < array.Length; i++)
                    {
                        str = str + string.Format("{0}: {1}\r\n", array[i], record[array[i]]);
                    }
                    str = str + "\r\n";
                }
                return str;
            }
            return "0 records returned.";
        }

        private void ParseData(string data, char lineSeparator, char fieldSeparator)
        {
            string[] strArray = data.Split(new char[] { lineSeparator });
            string[] fields = strArray[0].Split(new char[] { fieldSeparator });
            for (int i = 1; i < strArray.Length; i++)
            {
                if (strArray[i].Length > 0)
                {
                    string[] values = strArray[i].Split(new char[] { fieldSeparator });
                    base.Add(new DataRecord(fields, values));
                }
            }
        }
    }
}

