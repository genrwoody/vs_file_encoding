using System;
using System.Text;
using Microsoft.VisualStudio.Text;

namespace FileEncoding
{
    public static class EncodingExtensions
    {
        public static Encoding DefaultAsUtf8Encoding(this ITextDocument document)
        {
            // a little trick to fix the issue: When the encoding could be regarded as both utf8 and the default locale encoding (e.g. `GB2312`), but we would like to display it as `UTF-8` instead of the default.
            // Workaround: Change the encoding from default to utf8, but remains !isDirty and keep the LastContentModifiedTime.
            if (!document.IsDirty
                && !document.Encoding.HasBom()
                && document.Encoding.DisplayName() == Encoding.Default.DisplayName())
            {
                if (document.IsUtf8Encoding())
                {
                    //var isDirtyBefore = document.IsDirty;
                    //var lastContentModifiedTime = document.LastContentModifiedTime;
                    try
                    {
                        document.Encoding = new UTF8Encoding(false, true); //UTF-8 (without BOM)
                        // Keep it as not dirty to decrease unnecessary content modification
                        //if (!isDirtyBefore)
                        //{
                        //    //document.UpdateDirtyState(true, lastContentModifiedTime);
                        //    document.Save();
                        //}
                        //else
                        //{
                        //    document.UpdateDirtyState(false, lastContentModifiedTime);
                        //}
                    }
                    catch
                    {
                    }
                }
            }

            return document.Encoding;
        }

        private static bool IsUtf8Encoding(this ITextDocument document)
        {
            return true;
        }

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
