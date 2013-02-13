using System.Text;

namespace GeocachingPlus.Model.Api.GeocachingCom
{
    public class CryptUtils
    {
        public static string Rot13(string text)
        {
            if (text == null)
            {
                return "";
            }
            var result = new StringBuilder();
            // plaintext flag (do not convert)
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
