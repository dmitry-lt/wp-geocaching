using System.Text;

namespace GeocachingPlus.Model.Api.GeocachingCom
{
    public class HttpUtilityEx
    {
        public static string UrlEncodeUnicode(string str)
        {
            return UrlEncodeUnicode(str, false);
        }

        internal static string UrlEncodeUnicode(string value, bool ignoreAscii)
        {
            if (value == null)
            {
                return null;
            }
            int length = value.Length;
            StringBuilder stringBuilder = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                char c = value.ToCharArray()[i];
                if ((c & 'ﾀ') != '\0')
                {
                    stringBuilder.Append("%u");
                    stringBuilder.Append(IntToHex((int)((int)c >> 12 & '\u000f')));
                    stringBuilder.Append(IntToHex((int)((int)c >> 8 & '\u000f')));
                    stringBuilder.Append(IntToHex((int)((int)c >> 4 & '\u000f')));
                    stringBuilder.Append(IntToHex((int)(c & '\u000f')));
                }
                else
                {
                    if (ignoreAscii || IsUrlSafeChar(c))
                    {
                        stringBuilder.Append(c);
                        goto IL_D6;
                    }
                    if (c == ' ')
                    {
                        stringBuilder.Append('+');
                        goto IL_D6;
                    }
                    stringBuilder.Append('%');
                    stringBuilder.Append(IntToHex((int)((int)c >> 4 & '\u000f')));
                    stringBuilder.Append(IntToHex((int)(c & '\u000f')));
                }
            IL_D6:
                ;
            }
            return stringBuilder.ToString();
        }

        public static char IntToHex(int n)
        {
            if (n <= 9)
            {
                return (char)(n + 48);
            }
            return (char)(n - 10 + 97);
        }

        public static bool IsUrlSafeChar(char ch)
        {
            if (ch < 'a' || ch > 'z')
            {
                if (ch >= 'A' && ch <= 'Z')
                {
                    goto IL_1E;
                }
                if (ch < '0' || ch > '9')
                {
                }
                else
                {
                    goto IL_1E;
                }
                char c = ch;
                if (c != '!')
                {
                    switch (c - '(')
                    {
                        case 0:
                            {
                                goto IL_51;
                            }
                        case 1:
                            {
                                goto IL_51;
                            }
                        case 2:
                            {
                                goto IL_51;
                            }
                        case 3:
                            {
                                goto IL_53;
                            }
                        case 4:
                            {
                                goto IL_53;
                            }
                        case 5:
                            {
                                goto IL_51;
                            }
                        case 6:
                            {
                                goto IL_51;
                            }
                    }
                    if (c != '_')
                    {
                    }
                    else
                    {
                        goto IL_51;
                    }
                IL_53:
                    return false;
                }
            IL_51:
                return true;
            }
        IL_1E:
            return true;
        }
    }
}
