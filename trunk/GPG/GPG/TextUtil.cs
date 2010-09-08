namespace GPG
{
    using GPG.Logging;
    using System;
    using System.Collections.Generic;

    public static class TextUtil
    {
        public static char Capitalize(char letter)
        {
            return char.ToUpper(letter);
        }

        public static string Capitalize(string word)
        {
            return word.Insert(1, Capitalize(word[0]).ToString()).Remove(0, 1);
        }

        public static string DeleteWord(string text)
        {
            return DeleteWord(text, text.Length);
        }

        public static string DeleteWord(string text, int caretPosition)
        {
            try
            {
                if (((text != null) && (text.Length > 0)) && (caretPosition > 0))
                {
                    int startIndex = caretPosition - 1;
                    while (startIndex > 0)
                    {
                        if (IsWordBreakingChar(text[startIndex]))
                        {
                            startIndex++;
                            break;
                        }
                        startIndex--;
                    }
                    if (startIndex == caretPosition)
                    {
                        startIndex--;
                    }
                    return text.Remove(startIndex, caretPosition - startIndex);
                }
                return text;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return text;
            }
        }

        public static bool IsAlphaNumericChar(char key)
        {
            return IsAlphaNumericChar((int) key);
        }

        public static bool IsAlphaNumericChar(int key)
        {
            if ((((key <= 0x2f) || (key >= 0x3a)) && ((key <= 0x40) || (key >= 0x5b))) && (((key <= 0x60) || (key >= 0x7b)) && ((key != 0x5f) && (key != 0x2d))))
            {
                return false;
            }
            return true;
        }

        public static bool IsAlphaNumericString(string text)
        {
            return IsAlphaNumericString(text, new char[0]);
        }

        public static bool IsAlphaNumericString(string text, char[] exceptions)
        {
            for (int i = 0; i < text.Length; i++)
            {
                char key = text[i];
                if (!IsAlphaNumericChar(key) && (Array.IndexOf<char>(exceptions, key) < 0))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsDisplayChar(char key)
        {
            return IsDisplayChar((int) key);
        }

        public static bool IsDisplayChar(int key)
        {
            return ((key > 0x1f) && (key < 0x7f));
        }

        public static bool IsWordBreakingChar(char c)
        {
            if ((((c != ' ') && (c != '\'')) && ((c != '"') && (c != '.'))) && (((c != ';') && (c != ',')) && (c != '-')))
            {
                return false;
            }
            return true;
        }

        public static string[] SplitString(string text, int chunkSize)
        {
            List<string> list = new List<string>((text.Length / chunkSize) + 1);
            int startIndex = 0;
            while (startIndex < text.Length)
            {
                if (chunkSize >= (text.Length - startIndex))
                {
                    list.Add(text.Substring(startIndex, chunkSize));
                    startIndex += chunkSize;
                }
                else
                {
                    list.Add(text.Substring(startIndex, text.Length - startIndex));
                    startIndex += text.Length - startIndex;
                }
            }
            return list.ToArray();
        }

        public static string[] SplitString(string text, string splitter)
        {
            List<string> list = new List<string>();
            int startIndex = 0;
            int index = text.IndexOf(splitter, startIndex);
            while (index >= 0)
            {
                list.Add(text.Substring(startIndex, (index + 1) - startIndex));
                startIndex = index + 1;
                index = text.IndexOf(splitter, startIndex);
            }
            if (index < text.Length)
            {
                index = text.Length;
                list.Add(text.Substring(startIndex, index - startIndex));
            }
            return list.ToArray();
        }

        public static string TrimNonDisplayChars(string text)
        {
            List<char> list = new List<char>();
            for (int i = 0; i < 0x20; i++)
            {
                list.Add((char) ((ushort) i));
            }
            for (int j = 0x7f; j < 0xffff; j++)
            {
                list.Add((char) ((ushort) j));
            }
            return text.Trim(list.ToArray());
        }
    }
}

