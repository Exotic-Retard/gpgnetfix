namespace GPGnetCommunicationsLib.Utils
{
    using System;
    using System.Collections.Generic;

    public static class FormatData
    {
        public static void FormatDataset(List<string> columns, List<List<object>> rows)
        {
            int num = 0;
            List<int> list = new List<int>();
            foreach (string str in columns)
            {
                list.Add(str.Length + 1);
            }
            foreach (List<object> list2 in rows)
            {
                num = 0;
                foreach (object obj2 in list2)
                {
                    int num2 = obj2.ToString().Length + 1;
                    if (list[num] < num2)
                    {
                        list[num] = num2;
                    }
                    num++;
                }
            }
            num = 0;
            string str2 = "";
            foreach (string str3 in columns)
            {
                str2 = str2 + str3.PadLeft(list[num], ' ');
                num++;
            }
            Console.WriteLine(str2);
            foreach (List<object> list3 in rows)
            {
                num = 0;
                str2 = "";
                foreach (object obj3 in list3)
                {
                    str2 = str2 + obj3.ToString().PadLeft(list[num], ' ');
                    num++;
                }
                Console.WriteLine(str2);
            }
        }
    }
}

