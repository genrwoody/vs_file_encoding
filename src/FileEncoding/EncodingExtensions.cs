using System.Text;

namespace FileEncoding
{
    public static class EncodingExtensions
    {
        /// <summary>
        /// A BOM(byte-order mark) is used to indicate how a processor places 
        /// serialized text into a sequence of bytes.
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool HasBom(this Encoding encoding)
        {
            //return (encoding.GetPreamble()?.Length ?? 0) > 0;
            return encoding.GetPreamble().Length > 0;
        }

        /// <summary>
        /// A custom short name of Encoding
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static string ShortName(this Encoding encoding)
        {
            var charset = encoding.WebName + (encoding.HasBom() ? " bom" : "");
            return charset.ToUpper();
        }

        /// <summary>
        /// A custom short name of Encoding
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static string DisplayName(this Encoding encoding)
        {
            //var encodingName = "Unknown!";
            //var codePage = encoding.CodePage;
            //if (codePage == Encoding.UTF8.CodePage)
            //    encodingName = "UTF-8";
            //else if (codePage == Encoding.Unicode.CodePage)
            //    encodingName = "Unicode LE";
            //else if (codePage == Encoding.BigEndianUnicode.CodePage)
            //    encodingName = "Unicode BE";
            //else
            //    encodingName = encoding.EncodingName;

            //return encodingName + (encoding.HasBom() ? " BOM" : "");
            return encoding.EncodingName + (encoding.HasBom() ? " BOM" : "");
        }
    }
}
