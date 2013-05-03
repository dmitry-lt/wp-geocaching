using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text;

namespace GeocachingPlus.Model.Api.OpencachingDe
{
    public class Decrypt
    {
        public static string HintDecrypt(string text)
        {
            if (text == null)
            {
                return "";
            }
            var result = new StringBuilder();
            var plaintext = false;

            var length = text.Length;
            for (var index = 0; index < length; index++)
            {
                int c = text[index];
                if (c == '[')
                {
                    plaintext = true;
                }
                else if (c == ']')
                {
                    plaintext = false;
                }
                else if (!plaintext)
                {
                    var capitalized = c & 32;
                    c &= ~capitalized;
                    c = ((c >= 'A') && (c <= 'Z') ? ((c - 'A' + 13) % 26 + 'A') : c)
                            | capitalized;
                }
                result.Append((char)c);
            }
            return result.ToString();
        }
    }
}
