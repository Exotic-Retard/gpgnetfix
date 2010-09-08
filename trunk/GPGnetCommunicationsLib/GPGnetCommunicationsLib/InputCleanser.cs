namespace GPGnetCommunicationsLib
{
    using System;
    using System.Text.RegularExpressions;

    public static class InputCleanser
    {
        private static readonly string _emailPattern = @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$";
        private static readonly Regex _emailRegex = new Regex(_emailPattern, RegexOptions.IgnoreCase);
        private static readonly Regex _ipRegex = new Regex(_ipRegexPattern, RegexOptions.IgnoreCase);
        private static readonly string _ipRegexPattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";

        private static bool CheckRegexMatch(string matchString, Regex regex)
        {
            return regex.IsMatch(matchString);
        }

        public static string CleanseInput(string input, InputType inputType)
        {
            string str2;
            bool flag;
            string matchString = input.Trim();
            switch (matchString)
            {
                case null:
                case string.Empty:
                    return string.Empty;

                default:
                    str2 = string.Empty;
                    flag = true;
                    switch (inputType)
                    {
                        case InputType.EmailAddress:
                            flag = CheckRegexMatch(matchString, _emailRegex);
                            break;

                        case InputType.IpAddress:
                            flag = CheckRegexMatch(matchString, _ipRegex);
                            break;

                        case InputType.Sql:
                            str2 = CleanseSql(matchString, inputType);
                            break;

                        case InputType.SqlPassword:
                            str2 = CleanseSql(matchString, inputType);
                            break;
                    }
                    break;
            }
            if (flag)
            {
                return str2;
            }
            return string.Empty;
        }

        private static string CleanseSql(string originalIput, InputType type)
        {
            originalIput = originalIput.Replace("'", @"\'");
            if (type != InputType.SqlPassword)
            {
                originalIput = originalIput.Replace(";", "&semi");
            }
            else
            {
                originalIput = originalIput.Replace(";", @"\;");
            }
            originalIput = originalIput.Replace("\"", "\\\"");
            originalIput = originalIput.Replace("--", @"\-\-");
            return originalIput;
        }

        public static string[] CleanseSqlArgs(object[] args)
        {
            string[] strArray = new string[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                strArray[i] = CleanseInput(args[i].ToString(), InputType.Sql);
                if (args[i] == null)
                {
                    args[i] = string.Empty;
                }
            }
            return strArray;
        }
    }
}

